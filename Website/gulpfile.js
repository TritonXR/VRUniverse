var gulp = require('gulp'),
  sass = require('gulp-sass'),
  rename = require('gulp-rename'),
  nodemon = require('gulp-nodemon'),
  test2 = require('browser-sync').create();
  gulpif = require('gulp-if');

const paths = {
  src: [
    './app.js',
    './routes/*.js',
    './scss/**/*.scss'
  ]
};

gulp.task('css', function () {
  let stream = gulp.src('./scss/universe.scss')
    .pipe(sass().on('error', sass.logError))
    .pipe(gulp.dest('./public/stylesheets/'))
    .pipe(rename({suffix: '.min'}))
    .pipe(gulp.dest('./public/stylesheets/'));
  return stream;
});

gulp.task('nodemon', function(cb) {
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
      gulp.task('css');
    });
});

gulp.task('default',
  gulp.series('css', 'nodemon'),
  function () {
    gulp.watch('./scss/**/*.scss', ['css']);
  });