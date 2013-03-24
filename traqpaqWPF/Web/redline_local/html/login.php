<?php
// This file is used as an abstraction layer for the traqpaq C# application. Login details are passed via the $_POST parameter
// and sent to the MySQL databse.

// check if POST params are set
if (!isset($_POST['user']) || !isset($_POST['pass'])) {
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

// check username and password combo using bcrypt->verify
$uname = $_POST['user'];
$pass = $_POST['pass'];
try {
	$stmt = $dbh->prepare("SELECT paswd FROM user WHERE email = :uname");
	$stmt->execute(array('uname' => $uname));

	$row = $stmt->fetch();
	if ($row) {
		// username exists, but check password
		$bcrypt = new Bcrypt();
		if ($bcrypt->verify($pass, $row[0])) {
			// correct password, create session
			echo "LOGGED IN";
		} else {
			die("Incorrect password");
		}
	} else {
		echo "Username not found";
	}
} catch(PDOException $e) {
	echo 'ERROR: ' . $e->getMessage();
}