using System.Text.Json;
using System.Web;

namespace TFG.Application.Services.OpenProjectIntegration
{
	public class OpenProjectFilterBuilder
	{
		private readonly Dictionary<string, object> _filters = new();
		private int? _offset;
		private int? _pageSize;
		private string? _sortBy;
		private string? _groupBy;
		private bool? _showSums;
		private string? _select;

		public OpenProjectFilterBuilder SetOffset(int offset)
		{
			_offset = offset;
			return this;
		}

		public OpenProjectFilterBuilder SetPageSize(int pageSize)
		{
			_pageSize = pageSize;
			return this;
		}

		public OpenProjectFilterBuilder AddFilter(string key, string @operator, params string[] values)
		{
			_filters[key] = new Dictionary<string, object> { { "operator", @operator }, { "values", values.Length > 0 ? values : null } };
			return this;
		}

		public OpenProjectFilterBuilder SetSortBy(string field, string order = "asc")
		{
			_sortBy = JsonSerializer.Serialize(new List<List<string>> { new() { field, order } });
			return this;
		}

		public OpenProjectFilterBuilder SetGroupBy(string groupBy)
		{
			_groupBy = groupBy;
			return this;
		}

		public OpenProjectFilterBuilder SetShowSums(bool showSums)
		{
			_showSums = showSums;
			return this;
		}

		public OpenProjectFilterBuilder SetSelect(params string[] fields)
		{
			_select = string.Join(",", fields);
			return this;
		}

		public string Build(int projectId)
		{
			var query = HttpUtility.ParseQueryString(string.Empty);
			if (_offset.HasValue) query["offset"] = _offset.ToString();
			if (_pageSize.HasValue) query["pageSize"] = _pageSize.ToString();
			if (_sortBy != null) query["sortBy"] = _sortBy;
			if (_groupBy != null) query["groupBy"] = _groupBy;
			if (_showSums.HasValue) query["showSums"] = _showSums.ToString().ToLower();
			if (_select != null) query["select"] = _select;
			if (_filters.Count > 0) query["filters"] = JsonSerializer.Serialize(_filters);

			return $"/api/v3/projects/{projectId}/work_packages?{query}";
		}
	}
