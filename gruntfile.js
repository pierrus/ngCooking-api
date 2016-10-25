module.exports = function (grunt) {

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        // Exec pour l'exécution de commandes depuis l'invite de commandes Windows (pas Powershell!)
        exec: {
			test: {
				command: 'dotnet test ./src/apis.test'
			},
            rmdir: {
                command: 'if exist build rd build /S /Q',
                // Cheat car impossible de supprimer l'erreur si le répertoire existe (alors qu'il est bien supprimé)
                exitCodes: [0, 1]
            },
            mkdir: {
                command: 'mkdir build'
            },
            publish: {
                command: 'dotnet publish ./src/apis -o build'
            }
        },
		jshint: {
			all: ['src/apis/wwwroot/js/**/*.js']
		},
        concat: {
            js: {
                src: ['build/wwwroot/js/*.js',
                      '!build/wwwroot/js/<%= pkg.name %>.js',
                      '!build/wwwroot/js/<%= pkg.name %>.min.js'],
                dest: 'build/wwwroot/js/<%= pkg.name %>.js'
            },
            css: {
                src: ['build/wwwroot/css/*.css',
                      '!build/wwwroot/css/<%= pkg.name %>.css',
                      '!build/wwwroot/css/<%= pkg.name %>.min.css'],
                dest: 'build/wwwroot/css/<%= pkg.name %>.css'
            },
        },
        uglify: {
            options: {
                banner: '/*! <%= pkg.name %> <%= grunt.template.today("yyyy-mm-dd") %> */\n'
            },
            js: {
                src: ['build/wwwroot/js/<%= pkg.name %>.js'],
                dest: 'build/wwwroot/js/<%= pkg.name %>.min.js'
            }
        },
        cssmin: {
            css: {
                src: ['build/wwwroot/css/<%= pkg.name %>.css'],
                dest: 'build/wwwroot/css/<%= pkg.name %>.min.css'
            }
        },
        usemin: {
            html: 'build/wwwroot/index.html'
        }
    });

    // Load the plugin that provides the "uglify" task.
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-usemin');
    grunt.loadNpmTasks('grunt-exec');

    // task(s).
    grunt.registerTask('package', ['exec', 'concat', 'uglify', 'cssmin', 'usemin']);

};