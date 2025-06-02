using UnityEngine.AI;

public static class NavMeshAgentExtensions
{
    public static bool SafeStop(this NavMeshAgent agent)
    {
        if (IsAgentValid(agent))
        {
            agent.isStopped = true;
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

    public static bool IsAgentValid(NavMeshAgent agent)
    {
        return agent != null
               && agent.isActiveAndEnabled
               && agent.isOnNavMesh;
    }
}