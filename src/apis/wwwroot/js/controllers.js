var ngCookingControllers = angular.module('ngCookingControllers', []);

ngCookingControllers.controller('HomeCtrl', ['$scope', 'recipesService', 'communityService', '$rootScope',
  function ($scope, recipesService, communityService, $rootScope) {
	recipesService.getRecipes().then(
	  function(recipes) {
	    $scope.recipes = recipes;
	  }
	);
	
	communityService.getAuthenticationStatus().then(
	  function(data) {
		$scope.showAddNewRecipe = data.loggedIn;
	  }
	);
	
	$rootScope.$on('authenticationStatusChange', function(event, loggedInStatus) {
	  console.log('logged in event updated');
	  $scope.showAddNewRecipe = loggedInStatus;
	});
	
	$scope.getNumber = recipesService.getNumber;
  }]);

ngCookingControllers.controller('RecipesCtrl', ['$scope', 'recipesService',
  function($scope, recipesService) {
    recipesService.getRecipes().then(
	  function(recipes) {
		$scope.recipes = recipes;
	  }
	);
	
	$scope.filterIngredients = function(item)
	{
	  if ($scope.ingredientsSearch === '' || $scope.ingredientsSearch === undefined || !$scope.ingredientsSearch || $scope.ingredientsSearch === null) {
	    return true;
	  }
	  else
	  {
	    var searchedIngredients = $scope.ingredientsSearch.split(' ');
		var displayItem = true;
		
		$.each(searchedIngredients, function(index, searchedIngredient) {
		  var foundSearchedIngredient = false;
		  $.each(item.ingredients, function(index2, recipeIngredient) {
		    if (recipeIngredient.indexOf(searchedIngredient) !== -1) {
			  foundSearchedIngredient = true
			}
		  });
		  
		  if (foundSearchedIngredient === false)
		    displayItem = false;
			
		});
	  
	    return displayItem;
	  }
	};
	
	$scope.caloriesGreaterThan = function(item) {
		if ($scope.minCalories === undefined) {
		  return true;
		}
		else if (item['calories'] > $scope.minCalories) {                
			return true;
		} else {
			return false;
		}
	};
	
	$scope.caloriesLessThan = function(item) {
		if ($scope.maxCalories === undefined) {
		  return true;
		}
		else if (item['calories'] < $scope.maxCalories) {                
			return true;
		} else {
			return false;
		}
    };
	
	$scope.getNumber = recipesService.getNumber;
  }]);
  
ngCookingControllers.controller('LayoutCtrl', ['$scope', 'recipesService', '$location',
  function ($scope, recipesService, $location) {
	recipesService.getRecipes().then(
	  function(recipes)
	  {
	    if (!$scope.recipesCount)
	      $scope.recipesCount = recipes.length;
	  }
	);
	
	$scope.path = $location.$$path;
	
	$scope.$on('$routeChangeSuccess', function(event, current, previous, rejection) {
	  $scope.path = $location.$$path;
    });
}]);

ngCookingControllers.controller('RecipeNewCtrl', ['$scope', 'recipesService', 'communityService',
  function($scope, recipesService, communityService) {
	
	recipesService.getIngredientsCategories().then(
	  function(ingredientsCategories) { $scope.ingredientsCategories = ingredientsCategories; }
	);
	
	recipesService.getIngredients().then(
	  function(ingredients) { $scope.ingredients = ingredients; }
	);
	
	$scope.recipe = {};
	$scope.recipe.calories = 0;
	$scope.recipe.ingredients = new Array();
	
	$scope.addIngredient = function(ingredient) {
	
	  if (ingredient === null || ingredient === undefined)
	    return;
	
	  $scope.recipe.ingredients.push(ingredient);
	  $scope.recipe.calories += ingredient.calories;
    };
	
	$scope.removeIngredient = function(index) {
	  ingredient = $scope.recipe.ingredients[index];
	  $scope.recipe.calories -= ingredient.calories;
	  $scope.recipe.ingredients.splice(index, 1);
    };
	
	$scope.addRecipe = function(recipe) {
	
	  /* Basic info */
	  if (recipe.name === null || recipe.name === undefined || recipe.category === null || recipe.category === undefined
	      || recipe.preparation === null || recipe.preparation === undefined)
	  {
	    $scope.recipe.infoError = true;
		return;
	  }	  
	  $scope.recipe.infoError = false;
	
	  /* Ingredients */
	  if (recipe.ingredients.length === 0)
	  {
	    recipe.ingredientsError = true;
		return;
	  }
	  recipe.ingredientsError = false;
	
	  /* Authentication status */
	  communityService.getAuthenticationStatus().then(function(status)
	  {
	    if (status.loggedIn === false)
		{
		  $scope.recipe.authenticationError = true;
		  return;
		}
		
		$scope.recipe.authenticationError = false;
		
		recipe.creatorId = status.member.id;
	
	    $scope.recipe.memberId = status.member.id;
		$scope.recipe.success = recipesService.addRecipe(recipe);
	  })
	};
	
  }]);
  
 ngCookingControllers.controller('IngredientsCtrl', ['$scope', 'ingredientsService',
  function($scope, ingredientsService) {
    
	ingredientsService.getIngredients().then(
	  function(ingredients) { $scope.ingredients = ingredients; }
	);
	
	ingredientsService.getCategories().then(
	  function(categories) { $scope.categories = categories; }
	);
	
	$scope.caloriesGreaterThan = function(item) {                          
		if ($scope.minCalories === undefined) {
		  return true;
		}
		else if (item['calories'] > $scope.minCalories) {                
			return true;
		} else {                
			return false;
		}
	};
	
	$scope.caloriesLessThan = function(item) {
		if ($scope.maxCalories === undefined) {
		  return true;
		}
		else if (item['calories'] < $scope.maxCalories) {                
			return true;
		} else {                
			return false;
		}
    };
	
	$scope.categoryEquals = function(item) {
		if ($scope.category === '' || $scope.category === undefined || !$scope.category || $scope.category === null) {
		  return true;
		}
		else if (item['category'] === $scope.category) {                
			return true;
		} else {                
			return false;
		}
	};
	
  }]);
  
 ngCookingControllers.controller('CommunityCtrl', ['$scope', 'communityService', 'recipesService',
  function ($scope, communityService, recipesService) {
	communityService.getMembers().then(
	  function(members) { $scope.members = members; }
	);
	
	$scope.getNumber = recipesService.getNumber;
  }]);
  
 ngCookingControllers.controller('RecipeCtrl', ['$scope', 'communityService', 'recipesService', '$routeParams', '$location', '$sce',
  function ($scope, communityService, recipesService, $routeParams, $location, $sce) {
	
	$scope.recipeId = $routeParams.recipeId;
	
	var res = recipesService.getRecipe($routeParams.recipeId);
	res.then(function(recipe)
	  {
	    $scope.recipe = recipe;
		$scope.recipe.preparationHtml = $sce.trustAsHtml(recipe.preparation);
	    if ($scope.recipe === null || $scope.recipe === undefined)
		  $location.path('/recipes')
	  }
	);
	
	var res = recipesService.getRecipes();
	res.then(function(recipes)
	{
	    $scope.recipes = recipes;
	});
	
	$scope.getNumber = recipesService.getNumber;
	
	$scope.hideCurrentRecipe = function(item)
	{
	  return item.id != $scope.recipeId;
	};
	
	$scope.addComment = function(comment){
	
	  if (!comment)
	    return;
	  
	  communityService.getAuthenticationStatus().then(function(status) {
	  
	    if (status.loggedIn === false)
		  return;
		  
		comment.recetteId = $scope.recipe.id;
	    comment.userId = status.member.data.Id;
	    var recipe = recipesService.addNewComment(comment);
	    $scope.recipe.comments = recipe.comments;
	    $scope.recipe.mark = recipe.mark;
	  });
	};	
  }]);
  
ngCookingControllers.controller('LoginCtrl', ['$scope', 'communityService', 'recipesService', '$routeParams', '$location', '$rootScope',
  function ($scope, communityService, recipesService, $routeParams, $location, $rootScope) {
    
	/* Login method */
	$scope.login = function(email, password) {
	  communityService.login(email, password).then(function() {
		  communityService.getAuthenticationStatus().then(function(status){
			if (status.loggedIn === true) {
			  $scope.displayLoginModal = false;
			  $scope.loggedIn = true;
			  $scope.memberId = status.member.id;
			}
			else {
			  $scope.loggedIn = false;
			  $scope.memberId = null;
			}
			
			$scope.$emit('authenticationStatusChange', status.loggedIn);
		  });
	   });
	};
	
	/* Login init */
	communityService.getAuthenticationStatus().then(function(status){
	  if (status.loggedIn === true) {
		$scope.loggedIn = true;
		$scope.memberId = status.member.id;
	  }
      else {
		$scope.loggedIn = false;
		$scope.memberId = null;
	  }
	  
	  $scope.$emit('authenticationStatusChange', status.loggedIn);
	});
	$scope.displayLoginModal = false;
	
	/* Logout method */
	$scope.logout = function() {
	  communityService.logout();
	  $scope.loggedIn = false;
	  $scope.memberId = null;
	  $scope.$emit('authenticationStatusChange', false);
	};
	
	$scope.showLoginPopin = function() {
	  $scope.displayLoginModal = true;
	};
	
	$scope.hideLoginPopin = function() {
	  $scope.displayLoginModal = false;
	};
  }]);
  
ngCookingControllers.controller('CommunityDetailCtrl', ['$scope', 'communityService', 'recipesService', '$routeParams', '$location',
  function ($scope, communityService, recipesService, $routeParams, $location) {
  
    $scope.memberId = $routeParams.memberId;
	$scope.getNumber = recipesService.getNumber;
  
	communityService.getMember($routeParams.memberId).then(function(member) {
		if (member === null || member === undefined)
			$location.path('/community');
	
		$scope.member = member;
	
		recipesService.getUserRecipes($routeParams.memberId).then(function(userRecipes) {
		  $scope.recipes = userRecipes;
		});
	});
  }]);