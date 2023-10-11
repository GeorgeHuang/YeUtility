using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace YeUtility
{
    [BurstCompile]
    public struct CalculateDistanceJobs : IJobParallelFor, IDisposable
    {
        [ReadOnly] public NativeArray<Vector3> SourcePosList;

        [NativeDisableParallelForRestriction] [WriteOnly]
        public NativeArray<float> Result;

        private int _length;

        public void Setup(List<Vector3> input)
        {
            _length = input.Count;
            SourcePosList = new NativeArray<Vector3>(input.ToArray(), Allocator.TempJob);
            Result = new NativeArray<float>(_length * _length, Allocator.TempJob);
        }

        public void Setup(NativeArray<Vector3> input)
        {
            _length = input.Length;
            SourcePosList = new NativeArray<Vector3>(input, Allocator.TempJob);
            Result = new NativeArray<float>(_length * _length, Allocator.TempJob);
        }

        [BurstCompile]
        public void Execute(int index)
        {
            var myPos = SourcePosList[index];
            for (var i = 0; i < _length; ++i) Result[index * _length + i] = (myPos - SourcePosList[i]).magnitude;
        }

        public void Execute_old(int index)
        {
            var myI = index / _length;
            var tarI = index % _length;
            var myPos = SourcePosList[myI];
            var tarPos = SourcePosList[tarI];
            var rv = Vector3.Distance(myPos, tarPos);
            Result[index] = rv;
        }

        public void Dispose()
        {
            if (SourcePosList.IsCreated)
                SourcePosList.Dispose();
            if (Result.IsCreated)
                Result.Dispose();
        }
    }

    [BurstCompile]
    public struct GetTransInfoJob : IJobParallelForTransform, IDisposable
    {
        [WriteOnly] public NativeArray<Vector3> PosList;

        public void Dispose()
        {
        }

        public void Execute(int index, TransformAccess transform)
        {
            //Debug.Log($"indexa {index} {PosList.Length}");
            PosList[index] = transform.position;
        }
    }

    [BurstCompile]
    public struct SetTransInfoJob : IJobParallelForTransform, IDisposable
    {
        [ReadOnly] public NativeArray<Vector3> PosList;

        public void Dispose()
        {
        }

        public void Execute(int index, TransformAccess transform)
        {
            transform.position = PosList[index];
        }
    }

    [BurstCompile]
    public class TransAccessJob : IDisposable
    {
        private GetTransInfoJob getTransInfoJob;
        private JobHandle getTransInfoJobHandle;
        private int length;
        private NativeArray<Vector3> posList;
        private Vector3[] resultPosArray;
        private SetTransInfoJob setTransInfoJob;
        private JobHandle setTransInfoJobHandle;
        private TransformAccessArray transformAccessArray;

        public ref NativeArray<Vector3> PosList => ref posList;

        public void Dispose()
        {
            if (transformAccessArray.isCreated)
                transformAccessArray.Dispose();
            if (posList.IsCreated)
                posList.Dispose();
            getTransInfoJob.Dispose();
            setTransInfoJob.Dispose();
        }

        public void SetPos(Vector3[] v)
        {
            if (posList.IsCreated)
                posList.Dispose();
            posList = new NativeArray<Vector3>(v, Allocator.TempJob);
        }

        public void Setup(Transform[] transforms)
        {
            length = transforms.Length;
            transformAccessArray = new TransformAccessArray(transforms);
            posList = new NativeArray<Vector3>(length, Allocator.TempJob);
        }

        public void ExcuteGet(bool autoComplete = true)
        {
            getTransInfoJob = new GetTransInfoJob();
            getTransInfoJob.PosList = posList;
            getTransInfoJobHandle = getTransInfoJob.Schedule(transformAccessArray);
            if (autoComplete)
                GetComplete();
        }

        public ref Vector3 GetPos(int index)
        {
            //return posList[index];
            return ref resultPosArray[index];
        }

        public void SetPos(Vector3 pos, int index)
        {
            //posList[index] = pos;
            resultPosArray[index] = pos;
        }

        public void GetComplete()
        {
            getTransInfoJobHandle.Complete();
            resultPosArray = getTransInfoJob.PosList.ToArray();
        }

        public void ExcuteSet(bool autoComplete = true)
        {
            setTransInfoJob = new SetTransInfoJob();
            SetPos(resultPosArray);
            setTransInfoJob.PosList = posList;
            setTransInfoJobHandle = setTransInfoJob.Schedule(transformAccessArray);
            if (autoComplete)
                SetComplete();
        }

        public void SetComplete()
        {
            setTransInfoJobHandle.Complete();
        }
    }
}