using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeaguesStandingsTableProject.ViewModels
{
	public class TestClass
	{
        public int Id { get; set; }
        public string Name { get; set;}

    }

	public class TestClass2 
	{
		private readonly TestClass test;

        public TestClass2()
        {
            test = new TestClass();
        }

        public TestClass2 WithSomeTest(string personName)
        {
            test.Name = personName;

            return this;
        }
    }
}
