
namespace treenirepository.Models
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
      ContentData = exerciseData.ContentData;
      Sections = exerciseData.Sections.Select(s => new Section(s)).ToList();

    }
    [DataMember(Name = "id")]
    public int Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "created")]
    public DateTime Created { get; set; }
    [DataMember(Name = "contentData")]
    public string ContentData { get; set; }
    [DataMember(Name = "sections")]
    public ICollection<Section> Sections { get; set; }
  }
}