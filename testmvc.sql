-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 24-04-2023 a las 22:38:22
-- Versión del servidor: 10.4.27-MariaDB
-- Versión de PHP: 8.0.25

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `testmvc`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `Id` int(11) NOT NULL,
  `fechaInicio` datetime NOT NULL,
  `fechaFinalizacion` datetime NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  `Precio` double(10,0) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`Id`, `fechaInicio`, `fechaFinalizacion`, `InquilinoId`, `InmuebleId`, `Precio`, `Estado`) VALUES
(28, '2023-04-21 17:47:45', '2023-04-21 17:48:39', 4, 23, 10000, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `Id` int(11) NOT NULL,
  `Direccion` varchar(50) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Superficie` int(11) NOT NULL,
  `Latitud` decimal(10,0) NOT NULL,
  `Longitud` decimal(10,0) NOT NULL,
  `PropietarioId` int(11) NOT NULL,
  `Precio` decimal(10,0) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`Id`, `Direccion`, `Ambientes`, `Superficie`, `Latitud`, `Longitud`, `PropietarioId`, `Precio`, `Estado`) VALUES
(21, 'Cordoba', 1, 1, '4', '2', 5, '4223', 1),
(22, 'Mendoza', 3, 3, '33', '66', 5, '500', 1),
(23, 'Buenos Aires', 2, 2, '3', '3', 6, '4', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Telefono` varchar(50) NOT NULL,
  `Dni` varchar(50) NOT NULL,
  `Email` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`Id`, `Nombre`, `Apellido`, `Telefono`, `Dni`, `Email`) VALUES
(4, 'Claudio', 'Duvara', '2665024712', '44358778', 'duvaraclaudiojavier@gmail.com'),
(5, 'Javier', 'Castro', '32131231', '231313', '3131@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `Id` int(11) NOT NULL,
  `FechaPago` datetime NOT NULL,
  `Importe` decimal(10,0) NOT NULL,
  `ContratoId` int(11) NOT NULL,
  `activo` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`Id`, `FechaPago`, `Importe`, `ContratoId`, `activo`) VALUES
(3, '2023-04-17 21:56:00', '533', 19, 0),
(4, '2023-04-20 22:03:00', '233', 20, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Telefono` varchar(50) NOT NULL,
  `Dni` varchar(50) NOT NULL,
  `Email` varchar(50) NOT NULL,
  `Clave` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`Id`, `Nombre`, `Apellido`, `Telefono`, `Dni`, `Email`, `Clave`) VALUES
(5, 'Laura', 'Albornoz', '32131231', '4123412412', '3131@gmail.com', ''),
(6, 'Javier', 'Duvara', '2665024712', '44358778', 'duvaraclaudiojavier@gmail.com', '');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(45) NOT NULL,
  `Apellido` varchar(45) NOT NULL,
  `Email` varchar(45) DEFAULT NULL,
  `NombreUsuario` varchar(45) NOT NULL,
  `Contraseña` varchar(200) NOT NULL,
  `Avatar` varchar(45) DEFAULT NULL,
  `Rol` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`Id`, `Nombre`, `Apellido`, `Email`, `NombreUsuario`, `Contraseña`, `Avatar`, `Rol`) VALUES
(25, 'Claudio', 'Duvara', 'duvaraclaudiojavier@gmail.com', 'Duvarax', 'dJ3h1GWMjmcVxoQgvC6LJGnhxjV/amw1lEItDlSo4D0=', '/Uploads\\profile_avatar_25.jpg', 1),
(27, 'Laura', 'Albornoz', 'duvaraclaudiojavier1@gmail.com', '1234', 'QoAY4q9t5kcEdi8Wk8txbdGar9eutN/uZiErmXORarY=', '/Uploads\\profile_avatar_27.jfif', 2);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`Id`,`InquilinoId`,`InmuebleId`),
  ADD KEY `fk_contratos_inquilinos1` (`InquilinoId`),
  ADD KEY `fk_contratos_inmuebles1` (`InmuebleId`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `inmuebles_ibfk_1` (`PropietarioId`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`Id`,`ContratoId`),
  ADD KEY `fk_pagos_contratos` (`ContratoId`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
