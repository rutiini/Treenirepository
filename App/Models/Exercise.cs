
namespace Treenirepository.Models
{

  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;

  [DataContract(Name = "exercise")]
  public class Exercise
  {
    public Exercise()
    {
    }
    public Exercise(DataModels.Exercise exerciseData)
    {
      Id = exerciseData.Id;
      Name = exerciseData.Name;
      Created = exerciseData.Created != null ? exerciseData.Created : DateTime.Now;
      StartTime = exerciseData.StartTime;
      Sections = exerciseData.Sections.Select(s => new Section(s)).ToList();

    }
    [DataMember(Name = "id")]
    public int Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "created")]
    public DateTime Created { get; set; }
    [DataMember(Name = "startTime")]
    public DateTime StartTime { get; set; }
    [DataMember(Name = "sections")]
    public ICollection<Section> Sections { get; set; }
  }
}