﻿using System.ComponentModel.DataAnnotations;

namespace FluentValidationTest
{
	public class Person
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public int Age { get; set; }
	}
}
