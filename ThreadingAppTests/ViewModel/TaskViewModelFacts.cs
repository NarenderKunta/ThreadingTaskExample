using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace ThreadingApp.ViewModel.Tests
{
    [TestClass]
    public class TaskViewModelFacts
    {
        [TestMethod]
        public void TaskViewModel_InitializeContructor_AlwaysCreatesTheObject()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act & Assert
            sut.ShouldNotBeNull();
        }

        [TestMethod]
        public void TaskViewModel_InitializeContructor_AlwaysCreatesTheObject1()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            sut.ButtonTask1Command.Execute(null);
            sut.AddTaskCommand.Execute(null);

            // Assert
            sut.TaskList.Count.ShouldBe(0);
        }


        [TestMethod]
        public void TaskViewModel_InitializeContructor_AlwaysCreatesTheObject2()
        {
            // Arrange
            var sut = this.CreateSut();

            // Act
            sut.ButtonTask1Command.Execute(null);

            // Assert
            sut.TaskList.Count.ShouldBe(1);
        }

        private TaskViewModel CreateSut()
        {
            return new TaskViewModel();
        }
    }
}