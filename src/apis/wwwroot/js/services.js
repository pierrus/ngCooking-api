var ngCookingServices = angular.module('ngCookingServices', []);

ngCookingServices.factory('configService', [function($http, communityService, $q) {
  var configService = {};
  configService.apiUrl = '/api';
  
  configService.getUrl = function(resource) {
	return configService.apiUrl + '/' + resource;
  };
  
  return configService;
  
}]);

ngCookingServices.factory('recipesService', ['$http', 'communityService', '$q', 'configService', function($http, communityService, $q, configService) {
  var recipesService = {};
  recipesService.recipes = new Array();
  recipesService.ingredients = new Array();
  recipesService.members = new Array();
  recipesService.rawRecipes = new Array();
  recipesService.ingredientsCategories = new Array();
  
  /* Load recipes from server-side JSON */
  recipesService.getRecipes = function() {
  
    var deferred = $q.defer();
	
	/* $q.all permet de définir un handler exécuté suite aux 3 méthodes asynchrones de chargement AJAX */
	$q.all([recipesService.loadIngredients(), communityService.loadMembers(), recipesService.loadRecipes()])
	.then(function() {
	
	  if (recipesService.recipes.length > 0) {
	    deferred.resolve(recipesService.recipes);
		return;
	  }
	
	  recipesService.recipes = recipesService.rawRecipes;
	
	  /* Calcul des moyennes des notes et liste d'ingredients pour chaque recette */
	  $.each(recipesService.recipes, function(index, value) {
		var marksSum = 0;
		
		if (!value.comments)
			value.comments = new Array();
		
		$.each(value.comments, function(markIndex, commentValue) {
			marksSum += commentValue.mark;
		});
		
		if (value.comments.length > 0) {
			value.mark = marksSum / value.comments.length; }
		else {
			value.mark = 0; }
		
		value.index = index;
		
		recipeIngredients = value.ingredients;
		value.ingredients = new Array();

		$.each(recipesService.ingredients, function(index, currentIngredient) {
		  if ($.inArray(currentIngredient.name.toLowerCase(), recipeIngredients) != -1)
			value.ingredients.push(currentIngredient);
		});
	  });
	  
	  console.log ('Loading ' + recipesService.recipes.length + ' recipes from recipesService.');
	  
	  deferred.resolve(recipesService.recipes);
    });
	
	return deferred.promise;
  };

  recipesService.getUserRecipes = function(memberId) {
  
    var deferred = $q.defer();
    memberId = Number(memberId);
  
    recipesService.getRecipes().then(function(recipes) {
	  var userRecipes = new Array();
	  $.each(recipes, function(index, recipe) {
	    if (recipe.creatorId === memberId)
		{
		  userRecipes.push(recipe);
		}
	  });
	  
	  deferred.resolve(userRecipes);
	});
	
	return deferred.promise;
  };
  
  recipesService.getNumber = function(num) {
    if (num === undefined)
	  return new Array(0);
  
	return new Array(Math.round(num));
  };
  
  /* Load ingredients from server-side JSON */
  recipesService.getIngredients = function() {
  
    var deferred = $q.defer();
  
    var promise = this.loadIngredients();
  
	promise.then(function() {
	  ingredients = recipesService.ingredients;
	  
	  console.log('loading ' + recipesService.ingredients.length + ' ingredients from recipesService');
	  
	  deferred.resolve(recipesService.ingredients);
	});
	  
	return deferred.promise;
  };
  
  /* Load ingredients from server-side JSON + extract unique categories */
  recipesService.getIngredientsCategories = function(callback) {
  
    var deferred = $q.defer();
  
    var promise = this.loadIngredients();
  
	promise.then(function() {
	  var ingredients = recipesService.ingredients;
	  recipesService.ingredientsCategories = new Array();
	  
	  $.each(ingredients, function (index, value) {
		if ($.inArray(value.category, recipesService.ingredientsCategories) === -1)
			recipesService.ingredientsCategories.push(value.category);
	  });
	  
	  console.log('loading ' + recipesService.ingredientsCategories.length + ' ingredients categories from recipesService');

	  deferred.resolve(recipesService.ingredientsCategories);
	});
	  
	return deferred.promise;
  };
  
  /* Add new recipe in local array */
  recipesService.addRecipe = function(recipe) {
	recipe.calories = 0;
	
	$.each(recipe.ingredients, function(index, ingredient) {
	  recipe.calories += ingredient.calories;
	});
	
	recipe.comments = new Array();
    recipe.isAvailable = true;
	recipe.picture = 'img/recettes/tajine-de-poulet.jpg';
	recipe.id = recipe.name.toLowerCase().replace(' ', '-').replace('à', 'a').replace('é', 'e').replace('è', 'e').replace('ê', 'e');
	recipe.mark = 0;
	recipe.index = recipesService.recipes.length;
	
    recipesService.recipes.push(recipe);
	
	return true;
  };
  
  /* Load recipes from server-side JSON */
  recipesService.getRecipe = function(recipeId) {
  
    var deferred = $q.defer();
  
    $http.get(configService.getUrl('recettes?id=' + recipeId)).success(function(recipe) {
	
	  var marksSum = 0;
		
		if (!recipe.comments)
			recipe.comments = new Array();
		
		$.each(recipe.comments, function(markIndex, commentValue) {
			marksSum += commentValue.mark;
		});
		
		if (recipe.comments.length > 0) {
			recipe.mark = marksSum / recipe.comments.length; }
		else {
			recipe.mark = 0; }
		
		recipeIngredients = recipe.ingredients;
		recipe.ingredients = new Array();

		$.each(recipesService.ingredients, function(index, currentIngredient) {
		  if ($.inArray(currentIngredient.name.toLowerCase(), recipeIngredients) != -1)
			recipe.ingredients.push(currentIngredient);
		});
	
	  deferred.resolve(recipe);
	});
	
	return deferred.promise;	  
  };
  
  /* Add a new comment to a recipe */
  recipesService.addNewComment = function(comment) {
    
	var recipe = null;
	
	$.each(recipesService.recipes, function(index, currentRecipe) {
	  if (currentRecipe.id === comment.recetteId)
	  {
		  currentRecipe.comments.push({
		    userId: comment.userId,
		    title: comment.title,
		    comment: comment.comment,
		    mark: Number(comment.mark),
			member: communityService.getMember(comment.userId)
		  });
		  
		  var marksSum = 0;
		  $.each(currentRecipe.comments, function(index, commentValue) {
		    marksSum += commentValue.mark;
		  });
		  
		  if (currentRecipe.comments.length > 0) {
			currentRecipe.mark = marksSum / currentRecipe.comments.length; }
		  else {
			currentRecipe.mark = 0; }		    
		  
		  recipe = currentRecipe;
		  
		  var config = {
                headers : {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }
		  var data = $.param({comment: {
			comment: comment.comment,
			mark: Number(comment.mark),
			recetteId: comment.recetteId,
			title: comment.title,
			userId: comment.userId
		  }});
		  $http.put(configService.getUrl('commentaires'), data, config).then(
			function() { console.log('comment added OK'); deferred.resolve(); },
			function() { console.log('comment added FAILED'); deferred.resolve(); });
	  }
	});
	
	return recipe;	
  };
  
  recipesService.loadIngredients = function() {
    var deferred = $q.defer();
    $http.get(configService.getUrl('ingredients')).success(function(data) {
	  recipesService.ingredients = data;
	  deferred.resolve();
	});
	return deferred.promise;
  };
  
  recipesService.loadRecipes = function() {
    var deferred = $q.defer();
	
    $http.get(configService.getUrl('recettes')).success(function(data) {
	  recipesService.rawRecipes = data;
	  deferred.resolve();
	});
	
	return deferred.promise;
  };
  
  return recipesService;
}]);

ngCookingServices.factory('ingredientsService', ['$http', '$q', 'configService', function($http, $q, configService) {
  var ingredientsService = {};
  ingredientsService.ingredients = new Array();
  
  /* Load recipes from server-side JSON */
  ingredientsService.getIngredients = function() {
  
    var deferred = $q.defer();
  
    $http.get(configService.getUrl('ingredients')).success(function(data) {
	  ingredients = data;
	  
	  var maxCalories = 0;
	  
	  /* Calories max */
      $.each(ingredients, function(index, value) {
		if (value.calories > maxCalories)
		  maxCalories = value.calories;
	  });
	  
	  /* Calories percentage & ingredients similaires */
	  $.each(ingredients, function(index, value) {
		value.caloriesPercentage = (value.calories / maxCalories ) * 100;
		value.similarIngredients = new Array();
		
		$.each(ingredients, function(index2, value2) {
			if (value.category === value2.category && value.id != value2.id)	{
			  value.similarIngredients.push(value2);
			}
		});
		
	  });
	  
	  console.log ('Loading ' + ingredients.length + ' recipes from ingredientsService.');
	  	  
	  deferred.resolve(ingredients);
    });
	
	return deferred.promise;
  };
  
  /* Load ingredients from server-side JSON + extract unique categories */
  ingredientsService.getCategories = function() {
  
    var deferred = $q.defer();
  
	$http.get(configService.getUrl('ingredients')).success(function(data) {
	  var ingredients = data;
	  var categories = new Array();
	  
	  $.each(ingredients, function (index, value) {
		if ($.inArray(value.category, categories) === -1)
			categories.push(value.category);
	  });
	  
	  console.log('loading ' + categories.length + ' ingredients categories from ingredientsService');
	  
	  deferred.resolve(categories);
	});
	  
	return deferred.promise;
  };
  
  return ingredientsService;
}]);

ngCookingServices.factory('communityService', ['$http', '$q', '$cookies', 'configService', function($http, $q, $cookies, configService) {
  var communityService = {};
  communityService.members = new Array();
  
  /* Load members from server-side JSON */
  communityService.getMembers = function() {
    var deferred = $q.defer();
	
	communityService.loadMembers().then(function()
	{
	  deferred.resolve(communityService.members);
	});
	
	return deferred.promise;
  };
  
  communityService.loadMembers = function() {
  
    var deferred = $q.defer();
  
	$http.get(configService.getUrl('community')).success(function(data) {
		var members = data;
		
		if (communityService.members.length > 0)
		{
		  deferred.resolve();
		  return;
		}
		
		$.each(members, function(index, member) {
		  if (member.level === 3)
			member.levelLabel = 'Expert';
		  else if (member.level === 3)
			member.levelLabel = 'Intermediate';
		  else
			member.levelLabel = 'Beginner';
			
		  member.age = 2016 - Number(member.birth);
		});
		  
		console.log('loading ' + members.length + ' members from communityService');
		
		communityService.members = members;
		
		deferred.resolve();
	});
	
	return deferred.promise;
  };
  
  /* A refactoriser je n'aime pas le côté synchrone / asynchrone c'est imprévisible */
  communityService.getMember = function(memberId) {
    memberId = Number(memberId);  

	  var deferred = $q.defer();
	
	  this.getMembers().then(function (members) {
	    $.each(members, function(index, member)
	    {
		  if (member.id === memberId)
		  {
		    deferred.resolve(member);
		  }
	    });
		
	    deferred.resolve(null);
	  });
	  
	  return deferred.promise;
  };
  
  communityService.login = function(email, password) {
  
    var deferred = $q.defer();
  
	var data = $.param({
		email: email,
		password: password
	});
	
	var config = {
                headers : {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }
	
	$http.post(configService.getUrl('authenticate'), data, config).then(
		function() { console.log('login OK'); deferred.resolve(); },
		function() { console.log('login FAILED'); deferred.resolve(); });
	
	return deferred.promise;
  }
  
  communityService.logout = function() {
    $http.delete(configService.getUrl('authenticate'));
  }
  
  communityService.getAuthenticationStatus = function() {
  
    var deferred = $q.defer();
		
	var loggedInMember = null;
	
	$http.get(configService.getUrl('authenticate')).then(
	  function(data)
	  {
		console.log('Already logged in:' + data);
		loggedInMember = data;
		deferred.resolve({ loggedIn: true, member: loggedInMember });
	  },
	  function()
	  {
		console.log('Not logged in');
		deferred.resolve({ loggedIn: false });
	  }
    );
	
	return deferred.promise;
  }
  
  return communityService;
  }]);