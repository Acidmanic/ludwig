using System;
using System.Collections.Generic;
using Ludwig.Common.Utilities;
using Ludwig.Contracts.Models;
using Ludwig.IssueManager.Jira.Models;

namespace Ludwig.IssueManager.Jira.Services
{
    internal class JiraPriorityMap
    {


        public JiraPriority FindClosest(Priority priority, List<JiraPriority> jiraPriorities)
        {
            if (jiraPriorities == null || jiraPriorities.Count == 0)
            {
                return null;
            }
            
            var index = jiraPriorities.Count / 2;

            return FindClosest(priority, jiraPriorities, jiraPriorities[index]);
        }
        
        public JiraPriority FindClosest(Priority priority, List<JiraPriority> jiraPriorities, JiraPriority defaultValue)
        {
            if (jiraPriorities == null || jiraPriorities.Count == 0)
            {
                return defaultValue;
            }
            
            var maxScore = 0.0;
            var closest = defaultValue;

            foreach (var jiraPriority in jiraPriorities)
            {
                if (jiraPriority.Name.ToLower() == priority.Name.ToLower())
                {
                    return jiraPriority;
                }

                var score = CalculateClosenessScore(priority, jiraPriority,jiraPriorities);

                if (score > maxScore)
                {
                    maxScore = score;
                    closest = jiraPriority;
                }
            }

            return closest;
        }

        private double CalculateClosenessScore(Priority priority, JiraPriority jiraPriority,List<JiraPriority> jiraPriorities)
        {
            var stringDistance = LevenshteinEditDistance.Compute(priority.Name.ToLower(), jiraPriority.Name.ToLower());

            var stringCloseness = 1.0/(Math.Abs(stringDistance)+1);

            var indexDistance = Math.Abs(IndexOf(priority) - IndexOf(jiraPriority, jiraPriorities)) ;

            var indexCloseness = 1.0 / (indexDistance + 1);

            return stringCloseness*indexCloseness;
        }


        private int IndexOf(Priority priority)
        {

            for (int i = 0; i < Priority.Priorities.Count; i++)
            {
                var p = Priority.Priorities[i];

                if (p.Name == priority.Name)
                {
                    return i;
                }
            }

            return -1;
        }
        
        private int IndexOf(JiraPriority priority,List<JiraPriority> priorities)
        {

            for (int i = 0; i < priorities.Count; i++)
            {
                var p = priorities[i];

                if (p.Name.ToLower() == priority.Name.ToLower())
                {
                    return i;
                }
            }

            return -1;
        }
    }
}