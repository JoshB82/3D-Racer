using System.Collections.Generic;

namespace _3D_Racer
{
    public abstract class Group
    {
        public List<Group> Groups { get; private set; }
        public List<Mesh> Meshes { get; private set; }

        public Group()
        {
            Groups = new List<Group>();
            Meshes = new List<Mesh>();
        }

        /// <summary>
        /// Add a subgroup to the current group
        /// </summary>
        /// <param name="subgroup">Group to add</param>
        public void Add_Subgroup(Group subgroup)
        {
            Groups.Add(subgroup);
        }

        /// <summary>
        /// Remove a subgroup from the current group
        /// </summary>
        /// <param name="subgroup">Group to remove</param>
        /// <returns>The removed subgroup</returns>
        /*public Group Pop_Subgroup(Group subgroup)
        {

        }
        */

        public static List<Group> Get_All_Subgroups(Group search_group)
        {
            List<Group> all_groups = new List<Group>();
            Search_Subgroup(search_group, ref all_groups);
            return all_groups;
        }

        // CHEKC WHEN ZERO IN LIST?
        public static void Search_Subgroup(Group search_group, ref List<Group> all_groups)
        {
            foreach (Group group in search_group.Groups)
            {
                all_groups.Add(group);
                Search_Subgroup(search_group, ref all_groups);
            }
        }

        public static List<Mesh> Get_All_Meshes(Group search_group)
        {
            List<Mesh> all_meshes = new List<Mesh>();
            foreach (Mesh mesh in search_group.Meshes) all_meshes.Add(mesh);
            Search_Subgroup_Meshes(search_group, ref all_meshes);
            return all_meshes;
        }

        private static void Search_Subgroup_Meshes(Group subgroup, ref List<Mesh> all_meshes)
        {
            foreach (Mesh mesh in subgroup.Meshes) all_meshes.Add(mesh);
            foreach (Group group in subgroup.Groups) Search_Subgroup_Meshes(subgroup, ref all_meshes);
        }

        public void Join(Mesh mesh)
        {
            Meshes.Add(mesh);
        }

        // SPLIT
        public void Remove(Mesh to_remove)
        {
            Meshes.RemoveAll(mesh => mesh == to_remove);
        }
    }
}
