namespace MossadAPI.Models
{
    public class MossadView
    {
        public int TotalAgents { get; set; }
        public int TotalAgentsActivate { get; set; }
        public int TotalTargets { get; set; }
        public int TotalTargetsDead { get; set; }
        public int TotalMissions { get; set; }
        public int TotalMissionsActivate { get;set; }
        public double AgentsToTargets { get; set; }
        public double SAgentsToTargets { get;set; }
    }
}
