namespace Shared.DTOs.Projects
{
	public record FilteredProjectDto
	{
		public required Guid Id { get; set; }
		public required string Name { get; set; }
		public required DateTime CreatedAt { get; set; }
		public List<UserReferenceDto> Members { get; set; }
		public virtual bool Equals(FilteredProjectDto? other)
		=> other is not null && Id == other.Id;

		public override int GetHashCode() => Id.GetHashCode();
	}

	public record UserReferenceDto
	{
		public required string Id { get; set; }
		public required string Email { get; set; }
	}
}
