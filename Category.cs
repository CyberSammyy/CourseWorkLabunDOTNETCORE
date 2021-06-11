using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CourseWorkLabunDOTNETCORE
{
    public static class Categories
    {
        private static int instancesCounter { get; set; } = 1;
        private static int initializationsCounter { get; set; } = 0;
        private static List<Category> categories = new List<Category>();
        public static void InitCategories()
        {
            if(initializationsCounter == 0)
            {
                initializationsCounter++;
                categories.Add(new Category("Food"));
                categories.Add(new Category("Intertainment"));
                categories.Add(new Category("Transport"));
                categories.Add(new Category("Shopping"));
                categories.Add(new Category("Other"));
                categories = categories.Select(x => x).AsParallel().OrderBy(x => x).ToList();
            }
        }
        public static void EraseDuplicates()
        {
            categories.Distinct();
        }
        public static void AddCategory(string name)
        {
            foreach(var category in categories)
            {
                if (category.Name == name)
                    return;
            }
            instancesCounter--;
            if (instancesCounter == 0)
                categories.Add(new Category(name));
            else throw new MoreThanOneINstanceException("Only allowed instances!");
            instancesCounter++;
            categories = categories.Select(x => x).AsParallel().OrderBy(x => x).ToList();
        }
        public static void SetCategoryName(this Category category, string name)
        {
            category.SetName(name);
            categories = categories.Select(x => x).AsParallel().OrderBy(x => x).ToList();
        }
        public static void RemoveCategory(string name)
        {
            categories.Remove(categories.Find(x => x.Name == name));
            categories = categories.Select(x => x).AsParallel().OrderBy(x => x).ToList();
        }
        public static List<Category> GetCategories()
        {
            return categories;
        }
    }
    public sealed class Category : IComparable
    {
        public string Name { get; private set; }
        public Category(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
        internal void SetName(string name)
        {
            Name = name;
        }
        public int CompareTo(object obj)
        {
            if (Name.First() > (obj as Category).Name.First()) return 1;
            else if (Name.First() == (obj as Category).Name.First()) return 0;
            else return -1;
        }
    }
}
