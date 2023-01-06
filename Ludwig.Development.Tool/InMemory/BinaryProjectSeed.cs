using System.Collections.Generic;
using Ludwig.DataAccess.Models;

namespace Ludwig.Development.Tool.InMemory
{
    public class BinaryProjectSeed
    {
        
        
        public Project Project { get;  }
        
        public BinaryProjectSeed()
        {
            Project = new Project()
            {
                Description = "This is a test project that has two sub entities for each entity",
                Name = "BinarySeed",
                Iterations = new List<Iteration>
                {
                    new Iteration
                    {
                        Description = "This is the first Iteration of BinaryProject.",
                        Name = "Demo"
                    },
                    new Iteration
                    {
                        Description = "This is the last Iteration of BinaryProject.",
                        Name = "Release"
                    }
                },
                Goals = new List<Goal>
                {
                    new Goal()
                    {
                        Description = "This is the first Goal of Binary Project.",
                        Title = "First Goal",
                        Steps = new List<Step>
                        {
                            new Step
                            {
                                Description = "First Step Of First Goal",
                                Title = "S-11",
                                Tasks = new List<Task>
                                {
                                    new Task
                                    {
                                        Description = "First Task of S-11",
                                        Title = "T-111"
                                    },
                                    new Task
                                    {
                                        Description = "Second Task of S-11",
                                        Title = "T-112"
                                    },
                                }
                            },
                            new Step
                            {
                                Description = "Second Step Of First Goal",
                                Title = "S-12",
                                Tasks = new List<Task>
                                {
                                    new Task
                                    {
                                        Description = "First Task of S-12",
                                        Title = "T-121"
                                    },
                                    new Task
                                    {
                                        Description = "Second Task of S-12",
                                        Title = "T-122"
                                    },
                                }
                            }
                        }
                    },
                    new Goal()
                    {
                        Description = "This is the second Goal of Binary Project.",
                        Title = "Second Goal",
                        Steps = new List<Step>
                        {
                            new Step
                            {
                                Description = "First Step Of Second Goal",
                                Title = "S-21",
                                Tasks = new List<Task>
                                {
                                    new Task
                                    {
                                        Description = "First Task of S-21",
                                        Title = "T-211"
                                    },
                                    new Task
                                    {
                                        Description = "Second Task of S-21",
                                        Title = "T-212"
                                    },
                                }
                            },
                            new Step
                            {
                                Description = "Second Step Of Second Goal",
                                Title = "S-22",
                                Tasks = new List<Task>
                                {
                                    new Task
                                    {
                                        Description = "First Task of S-22",
                                        Title = "T-221"
                                    },
                                    new Task
                                    {
                                        Description = "Second Task of S-22",
                                        Title = "T-222"
                                    },
                                }
                            }
                        }
                    },
                }
            };
        }
        
        
    }
}