angular.module('todoApp', [])
  .controller('TodoController', ['$scope', '$http', function ($scope, $http) {
      $scope.todos = [];
      $scope.search = { Task: '' };

      $scope.addTodo = function () {
          var todo = { Task: $scope.task, IsDone: false, IsArchive: false };
          $http.post("/todo/create/", todo).success(function (result, status) {
              todo.Id = result;
              $scope.todos.push(todo);
              $scope.task = '';
          });
      };

      $scope.getTodos = function () {
          $http.get('/todo/getTodos').success(function (data) {
              $scope.todos = data;
          });
      }

      $scope.removeTodo = function (index) {
          $http.post("/todo/delete/", { id: $scope.todos[index].Id }).success(function (result, status) {
              $scope.todos.splice(index, 1);
          });
      }

      $scope.enterEditMode = function (todo) {
          todo.EditTask = todo.Task;
          todo.IsEdit = true;
      }

      $scope.confirmEdit = function (todo) {
          $http.post("/todo/edit/", { id: todo.Id, task: todo.EditTask }).success(function (result, status) {
              todo.Task = todo.EditTask;
              todo.IsEdit = false;
          });
      };

      $scope.cancelEdit = function (todo) {
          todo.IsEdit = false;
      };

      $scope.toggle = function (todo) {
          $http.post("/todo/toggle/", { id: todo.Id }).success(function (result, status) {
              todo.IsDone = result;
          });
      }

      $scope.remaining = function () {
          var count = 0;
          angular.forEach($scope.todos, function (todo) {
              count += todo.IsDone ? 1 : 0;
          });
          return count;
      };

      $scope.archive = function () {
          $http.post("/todo/archiveDone/").success(function (result, status) {
              $scope.getTodos();
          });
      };

      $(function () {
          $("#sortable").sortable({
              update: function (event, ui) {
                  var ids = $("#sortable").sortable("toArray");
                  $.post("/todo/changeOrder", $.param({ ids: ids }, true))
                  .done(function (data) {
                      $scope.todos = data;
                  }).fail(function () {
                      alert('排序尚未儲存!');
                  });
              }
          });
      });
  }]);