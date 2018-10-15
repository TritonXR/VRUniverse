var gulp = require('gulp'),
  sass = require('gulp-sass'),
  rename = require('gulp-rename'),
  nodemon = require('gulp-nodemon'),
  gutil = require('gulp-util'),
  gulpif = require('gulp-if');
let browserSync = gutil.env.production ?
  undefined : require('browser-sync').create();

const paths = {
  src: [
    './app.js',
    './routes/*.js',
    './scss/**/*.scss'
  ]
};


gulp.task('css', function () {
  gutil.log('Generating css');
  let stream = gulp.src('./scss/universe.scss')
    .pipe(sass().on('error', sass.logError))
    .pipe(gulp.dest('./public/stylesheets/'))
    .pipe(rename({suffix: '.min'}))
    .pipe(gulp.dest('./public/stylesheets/'));
  return stream;
});

gulp.task('nodemon', ['css'], function(cb) {
  return nodemon({
    exec: 'node',
    script: './bin/www',
    ext: 'js',
    watch: paths.src,
  })
    .once('start', function() {
      cb();
    })
    .on('restart', function() {
      gulp.start('css');
    });
});

gulp.task('default', ['css', 'nodemon'], function () {
  gulp.watch('./scss/**/*.scss', ['css']);
});

gulp.task('prod', ['css']);