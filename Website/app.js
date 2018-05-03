var express = require('express');
var path = require('path');
var favicon = require('serve-favicon');
var logger = require('morgan');
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');
var mongoose = require('mongoose');
var session = require('express-session');
var passport = require('passport');
var flash = require('connect-flash');
var validator = require('express-validator');

var index = require('./routes/index');
var user = require('./routes/user');
var projects = require('./routes/projects');
var download = require('./routes/download');
var upload = require('./routes/upload');
var filter = require('./routes/filter')

var app = express();
mongoose.connect('mongodb://rui:vruniverse@ds111478.mlab.com:11478/vruniverse');
require('./config/passport');
// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'jade');

// uncomment after placing your favicon in /public
//app.use(favicon(path.join(__dirname, 'public', 'favicon.ico')));
app.use(logger('dev'));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(validator());
app.use(cookieParser());
app.use(session({secret: 'secret7', resave: false, saveUninitialized: false}));
app.use(flash());
app.use(passport.initialize());
app.use(passport.session());
app.use("/public", express.static(path.join(__dirname, 'public')));
app.use("/data", express.static(path.join(__dirname, 'data')));

app.use('/', index);
app.use('/projects', projects);
app.use('/download', download);
app.use('/upload', upload);
app.use('/user/', user);
app.use('/filter', filter);


//debug and demoing
app.get('/2016.json', function(req, res, next){
	res.sendFile(__dirname + '/data/VRClubUniverseData/2016.json');
});

// catch 404 and forward to error handler
app.use(function(req, res, next) {
  var err = new Error('Not Found');
  err.status = 404;
  next(err);
});


// error handler
app.use(function(err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500);
  res.render('error');
});

module.exports = app;
