<?php
	require "conf/config.php";	
	require "conf/common.php";
	try {
		$conn = new PDO("mysql:host=$host;dbname=$dbname", $username, $password);
		$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
		$name = $_POST['name'];
		$boffer = $_POST['boffer'];
		$lat = $_POST['lat'];
		$lng = $_POST['lng'];
		$category = $_POST['category'];
		$status = $_POST['status'];
		$new_bus = array(
			"name" => $name,
			"boffer" => $boffer,
			"lat" => $lat,
			"lng"  => $lng,
			"category" => $category,
			"status" => $status
		);
		$sql = sprintf(
			"INSERT INTO %s (%s) values (%s)",
			"business",
			implode(", ", array_keys($new_bus)),
			":" . implode(", :", array_keys($new_bus))
		);
		$statement = $conn->prepare($sql);
		if($statement->execute($new_bus))
		{
			$id = @current($conn->query("select id from business order by id desc limit 1")->fetch());
			echojson(array('success'=>$id));
		}else{
			echojson(array('success'=>0));
		}	
	}
	catch(PDOException $e) {
		echo "Error: " . $e->getMessage();
	}
	$conn = null;
?>