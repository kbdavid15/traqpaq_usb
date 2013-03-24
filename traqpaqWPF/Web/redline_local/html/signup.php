<?php
// This file is used as an abstraction layer for the traqpaq C# application. Login details are passed via the $_POST parameter
// and sent to the MySQL databse.
// Variables expected in the POST array are username/email, password, 

// check if POST params are set
if (!isset($_POST['email']) || !isset($_POST['password'])) {
	header('HTTP/1.1 403 Forbidden');
	echo "<title>403 Forbidden</title>";
	echo "<h1>403 Forbidden</h1>";
	echo "You do not have permission to access this page.<hr>";
	die("<i>" . $_SERVER['SERVER_SOFTWARE'] . " Server at " . $_SERVER['SERVER_NAME'] . "</i>");
}

require_once 'functions.php';	// includes db login details, connection setup, bcrypt object

// connect to the database
try {
    $dbh = new PDO("mysql:host=$dbhost;dbname=$dbname", $dbuser, $dbpass);
    $dbh->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);
	$dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
} catch (PDOException $e) {
    print "Error!: " . $e->getMessage() . "<br>";
    die();
}

// get the variables out of the POST array
// check if firstname or last name are empty strings. Name is not required
if (empty($_POST['firstname'])) {
	$fname = NULL;
} else {
	$fname = $_POST['firstname'];
}
if (empty($_POST['lastname'])) {
	$lname = NULL;
} else {
	$lname = $_POST['lastname'];
}
if (empty($_POST['email'])) {
	die("Email required");
} else {
	$email = $_POST['email'];
}
if (empty($_POST['password'])) {
	die("Password required");
} else {
	$pass = $_POST['password'];
}

try {
	// hash password
	$bcrypt = new Bcrypt();
	$hash = $bcrypt->hash($pass);

	// prepare MySQL statement
	$stmt = $dbh->prepare("INSERT INTO user (firstname, lastname, email, paswd) VALUES (:fname, :lname, :email, :paswd)");
	$stmt->execute(array(
		'fname' => $fname,
		'lname' => $lname,
		'email' => $email,
		'paswd' => $hash ));
	echo "Success! Thanks for creating an account";
} catch (PDOException $e) {
	echo 'ERROR: ' . $e->getMessage();
}