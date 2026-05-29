namespace LearnPath.API.Algorithms.Recommendation;

public class RecommendationScore
{
    public Guid PathId { get; set; }
    public double Score { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public static class RecommendationEngine
{
    public static List<RecommendationScore> CalculateRecommendations(
        HashSet<Guid> completedModuleIds,
        HashSet<string> userTags,
        IEnumerable<(Guid PathId, string? Tags, int ModuleCount, double AvgRating)> candidatePaths)
    {
        var scores = new List<RecommendationScore>();

        foreach (var path in candidatePaths)
        {
            double score = 0;
            var reason = "Recommended";

            score += path.AvgRating * 0.3;

            if (!string.IsNullOrWhiteSpace(path.Tags))
            {
                var pathTags = path.Tags.Split(',', StringSplitOptions.TrimEntries);
                var matchCount = pathTags.Count(t => userTags.Contains(t.ToLowerInvariant()));
                if (matchCount > 0)
                {
                    score += matchCount * 0.2;
                    reason = "Matches your interests";
                }
            }

            if (path.ModuleCount > 0)
                score += Math.Min(path.ModuleCount / 20.0, 0.5);

            scores.Add(new RecommendationScore
            {
                PathId = path.PathId,
                Score = Math.Min(score, 1.0),
                Reason = reason
            });
        }

        return scores.OrderByDescending(s => s.Score).ToList();
    }
}
