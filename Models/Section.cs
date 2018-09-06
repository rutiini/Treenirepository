
namespace treenirepository.Models
{

  using System;
  using System.Runtime.Serialization;

  [DataContract(Name = "section")]
  public class Section
  {
    public Section()
    {
    }
    public Section(DataModels.Section sectionData)
    {
      Id = sectionData.Id;
      Name = sectionData.Name;
      Duration = sectionData.Duration;
      SetupDuration = sectionData.SetupDuration;
      Color = sectionData.Color;
      ExerciseId = sectionData.ExerciseId;

    }
    [DataMember(Name = "id")]
    public int Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "duration")]
    public int Duration { get; set; }
    [DataMember(Name = "setupDuration")]
    public int SetupDuration { get; set; }
    [DataMember(Name = "color")]
    public int Color { get; set; }
    [DataMember(Name = "exerciseId")]
    public int ExerciseId { get; set; }
    // public Exercise? Exercise { get; set; }
  }
}