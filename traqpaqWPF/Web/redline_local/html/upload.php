<?php
/*
This file is used as an abstraction layer for the traqpaq C# application. 
Login details are passed via the $_POST parameter and sent to the MySQL databse.
Description: Record data is uploaded via http request
*/

// first, make sure user is logged in
session_start();
echo $_SESSION['user'];
/*
if (!isset($_SESSION['user']) || empty($_SESSION['user'])) {
	header('HTTP/1.1 403 Forbidden');
	echo "<title>403 Forbidden</title>";
	echo "<h1>403 Forbidden</h1>";
	echo "You do not have permission to access this page.<hr>";
	die("<i>" . $_SERVER['SERVER_SOFTWARE'] . " Server at " . $_SERVER['SERVER_NAME'] . "</i>");
}
else
	echo $_SESSION['user'] . ": " . $_POST['sync'] ;


*/