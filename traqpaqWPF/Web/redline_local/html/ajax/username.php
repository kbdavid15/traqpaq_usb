<?php
/* 
This file is used as an abstraction layer for the traqpaq C# application.
A username is passed to this function and it checks the database to see if it exists or not
Returns:
	0: Username exists in database
	1: Username does not exist in database
	2: Empty string recieved
	3: PDO exception
*/

// check if POST params are set
if (!isset($_POST['email'])) {
	header('HTTP/1.1 403 Forbidden');
	echo "<title>403 Forbidden</title>";
	echo "<h1>403 Forbidden</h1>";
	echo "You do not have permission to access this page.<hr>";
	die("<i>" . $_SERVER['SERVER_SOFTWARE'] . " Server at " . $_SERVER['SERVER_NAME'] . "</i>");
}

$email = $_POST['email'];

if ($email == "") {
	die("Error[2]");
}

require_once '../functions.php';	// includes db login details, connection setup, bcrypt object

// connect to the database
try {
    $dbh = new PDO("mysql:host=$dbhost;dbname=$dbname", $dbuser, $dbpass);
    $dbh->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);
	$dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
} catch (PDOException $e) {
    die("Error[3]: " . $e->getMessage());
}

try {
	$stmt = $dbh->prepare("SELECT email FROM user WHERE email = :email");
	$stmt->execute(array('email' => $email));

	$row = $stmt->fetch();
	if ($row) {
		// username exists, but check password
		die("[0]");
	} else {
		// username not found
		die("[1]");
	}
} catch(PDOException $e) {
	echo 'ERROR: ' . $e->getMessage();
}