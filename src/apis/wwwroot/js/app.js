var ngCooking = angular.module('ngCooking', [
  'ngRoute',
  'ngCookingControllers',
  'ngCookingServices',
  'ngAnimate',
  'ngCookies'
]);

ngCooking.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/home', {
        templateUrl: 'partials/home.html',
        controller: 'HomeCtrl'
      }).
      when('/recipes', {
        templateUrl: 'partials/recipes.html',
        controller: 'RecipesCtrl'
      }).
	  when('/recipe/:recipeId', {
        templateUrl: 'partials/recipe.html',
        controller: 'RecipeCtrl'
      }).
	  when('/recipe-new', {
        templateUrl: 'partials/recipe-new.html',
        controller: 'RecipeNewCtrl'
      }).
	  when('/ingredients', {
        templateUrl: 'partials/ingredients.html',
        controller: 'IngredientsCtrl'
      }).
	  when('/community', {
        templateUrl: 'partials/community.html',
        controller: 'CommunityCtrl'
      }).
	  when('/member/:memberId', {
        templateUrl: 'partials/community-detail.html',
        controller: 'CommunityDetailCtrl'
      }).
      otherwise({
        redirectTo: '/home'
      });
  }]);