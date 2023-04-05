namespace Zadanie4.DTOs
{
    public class AnimalDTO
    {
        public AnimalDTO(int index, string name, string description, string category, string area)
        {
            Index = index;
            Name = name;
            Description = description;
            Category = category;
            Area = area;
        }

        public int Index { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
        
    }
}
