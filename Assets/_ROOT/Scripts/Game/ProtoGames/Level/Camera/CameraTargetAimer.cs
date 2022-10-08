namespace Ekstazz.ProtoGames.Level
{
    using Cinemachine;
    using UnityEngine;

    public class CameraTargetAimer : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup cameraTargetGroup;

        [Header("Settings")]
        [SerializeField] private float constantTargetWeight = 5;
        [SerializeField] private float commonTargetWeight = 1;
        
        private Transform constantTarget;

        
        public void AddConstantTarget(Transform target)
        {
            AddTarget(target, constantTargetWeight);
            constantTarget = target;
        }

        public void AddTarget(Transform target)
        {
            AddTarget(target, commonTargetWeight);
        }

        private void AddTarget(Transform target, float weight, float radius = 1)
        {
            cameraTargetGroup.AddMember(target, weight, radius);
        }

        public void ResetCommonTargets()
        {
            foreach (var t in cameraTargetGroup.m_Targets)
            {
                if (t.target != constantTarget)
                {
                    cameraTargetGroup.RemoveMember(t.target);
                }
            }
        }
    }
}
