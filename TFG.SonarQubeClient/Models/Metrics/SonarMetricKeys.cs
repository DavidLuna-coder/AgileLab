namespace TFG.SonarQubeClient.Models.Metrics
{
	public static class SonarMetricKeys
	{
		// 🔧 Mantenibilidad
		public const string CodeSmells = "code_smells";
		public const string SqaleRating = "sqale_rating";
		public const string TechnicalDebt = "sqale_index";
		public const string CognitiveComplexity = "cognitive_complexity";

		// 🔐 Seguridad
		public const string Vulnerabilities = "vulnerabilities";
		public const string SecurityRating = "security_rating";

		// ⚙️ Fiabilidad
		public const string Bugs = "bugs";
		public const string ReliabilityRating = "reliability_rating";

		// 📈 Cobertura y Tests
		public const string Coverage = "coverage";
		public const string LineCoverage = "line_coverage";
		public const string BranchCoverage = "branch_coverage";
		public const string Tests = "tests";
		public const string TestSuccessDensity = "test_success_density";

		// 🧬 Duplicación
		public const string DuplicatedLinesDensity = "duplicated_lines_density";
		public const string DuplicatedBlocks = "duplicated_blocks";

		// 📏 Tamaño del código
		public const string Lines = "lines";
		public const string Ncloc = "ncloc"; // Non-comment lines of code
		public const string Files = "files";
		public const string Classes = "classes";
		public const string Functions = "functions";

		// 📝 Documentación
		public const string CommentLines = "comment_lines";
		public const string CommentLinesDensity = "comment_lines_density";
	}
}
