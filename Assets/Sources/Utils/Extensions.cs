using UnityEngine;
using UnityEngine.AI;

namespace Assets.Sources.Utils
{
    public static class Extensions
    {
        public static bool SafeStop(this NavMeshAgent agent)
        {
            if (IsAgentValid(agent))
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                return true;
            }

            return false;
        }

        public static bool SafeDisable(this NavMeshAgent agent)
        {
            if (IsAgentValid(agent))
            {
                agent.isStopped = true;
                agent.enabled = false;
                return true;
            }

            return false;
        }

        public static bool SafeEnable(this NavMeshAgent agent)
        {
            if (IsAgentValid(agent))
            {
                agent.enabled = true;
                agent.isStopped = false;
                return true;
            }

            return false;
        }

        public static bool IsAgentValid(NavMeshAgent agent)
        {
            return agent != null && agent.isActiveAndEnabled;
        }

        public static TTarget SafeCast<TTarget>(this object source)
        {
            return source is TTarget target ? target : default;
        }
    }
}