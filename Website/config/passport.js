var passport = require('passport');
var User = require('../model/User');
var LocalStrategy = require('passport-local').Strategy;

passport.serializeUser(function(user, done) {
    done(null, user.id);
});

passport.deserializeUser(function(id, done) {
    User.findById(id, function(err, user) {
        done(err, user);
    });
});

passport.use('local.signup', new LocalStrategy({
    usernameField: 'username',
    passwordField: 'password',
    passReqToCallback: true
}, function(req, username, password, done) {
    req.checkBody('username', 'Enter username dumb ass').notEmpty();
    req.checkBody('password', 'Invalid password (min length: 4)').notEmpty().isLength({min: 4});
    req.checkBody('addcode', 'Invalid invite code').notEmpty().matches(/^vrclub$/);
    var errors = req.validationErrors();
    if(errors){
        var messages = [];
        errors.forEach(function(error) {
            messages.push(error.msg);
        });
        return done(null, false, req.flash('error', messages))
    }
    User.findOne({'username': username}, function(err, user) {
        if(err) {
            return done(err);
        }
        if(user) {
            return done(null, false, {message: 'Username already in use'});
        }
        var newUser = new User();
        newUser.username = username;
        newUser.password = password;
        newUser.save(function(err, result) {
            if(err) {
                return done(err);
            }
            return done(null, newUser);
        });
    });
}));

passport.use('local.signin', new LocalStrategy({
    usernameField: 'username',
    passwordField: 'password',
    passReqToCallback: true
}, function(req, username, password, done) {
    req.checkBody('username', 'Enter username dumb ass').notEmpty();
    req.checkBody('password', 'Please enter password').notEmpty();
    var errors = req.validationErrors();
    if(errors){
        var messages = [];
        errors.forEach(function(error) {
            messages.push(error.msg);
        });
        return done(null, false, req.flash('error', messages))
    }
    User.findOne({'username': username}, function(err, user) {
        if(err) {
            return done(err);
        }
        if(!user) {
            return done(null, false, {message: 'no such user'});
        }
        if(!user.validPassword(password)) {
            return done(null, false, {message: 'remember your fucking password retard'});
        }
        return done(null, user);
    });
}));
