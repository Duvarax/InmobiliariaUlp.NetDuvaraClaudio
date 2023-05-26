-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 26-05-2023 a las 22:19:56
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
-- Base de datos: `inmobiliariadb`
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
(39, '2023-05-26 16:47:00', '2023-06-09 16:47:00', 4, 40, 10000, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `Id` int(11) NOT NULL,
  `Direccion` varchar(50) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Uso` varchar(50) NOT NULL,
  `Tipo` varchar(50) NOT NULL,
  `Imagen` varchar(100) DEFAULT NULL,
  `PropietarioId` int(11) NOT NULL,
  `Precio` double NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`Id`, `Direccion`, `Ambientes`, `Uso`, `Tipo`, `Imagen`, `PropietarioId`, `Precio`, `Estado`) VALUES
(39, 'OJOM', 231, '321', '21', 'http://res.cloudinary.com/dhg4fafod/image/upload/v1685128206/se159qyif0ylf0admai9.jpg', 9, 10000, 1),
(40, 'San Juan', 5, 'Comercial', 'Local', 'http://res.cloudinary.com/dhg4fafod/image/upload/v1685128729/bexjmvllgr70r1gtvuzb.jpg', 9, 10000, 1),
(41, 'Jujuy', 2, 'Habitat', 'Vivienda', 'http://res.cloudinary.com/dhg4fafod/image/upload/v1685129024/zvhysc3vusphgx3gsut4.jpg', 9, 10000, 1);

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
(5, 'Javier', 'Castro', '32131231', '231313', '3131@gmail.com'),
(8, 'Cristian', 'Saccone', '2131321', '3123123', 'sca@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `Id` int(11) NOT NULL,
  `IdentificadorPago` int(11) NOT NULL,
  `FechaPago` datetime NOT NULL,
  `Importe` decimal(10,0) NOT NULL,
  `ContratoId` int(11) NOT NULL,
  `activo` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`Id`, `IdentificadorPago`, `FechaPago`, `Importe`, `ContratoId`, `activo`) VALUES
(18, 1, '2023-05-26 16:47:00', '232', 39, 0);

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
(6, 'Javier', 'Duvara', '2665024712', '44358778', 'duvaraclaudiojavier@gmail.com', ''),
(8, 'Mariano', 'Luzza', '3213123', '123123', 'mluza@gmail.com', ''),
(9, 'Montielazo', 'Je', '3122313', '321321', 'jemontiel06@gmail.com', '1234');

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
(25, 'Claudiox', 'Duvara', 'duvaraclaudiojavier@gmail.com', 'Duvarax', 'dJ3h1GWMjmcVxoQgvC6LJGnhxjV/amw1lEItDlSo4D0=', '/Uploads\\profile_avatar_25.jpg', 1),
(28, 'Laura', 'Albornoz', 'duvaraclaudiojavier1@gmail.com', 'Laurax', 'QoAY4q9t5kcEdi8Wk8txbdGar9eutN/uZiErmXORarY=', '/Uploads\\profile_avatar_28.jpeg', 2),
(29, 'Pedro', 'Pascal', 'pedro@gmail.com', 'pedroo', 'QoAY4q9t5kcEdi8Wk8txbdGar9eutN/uZiErmXORarY=', '/Uploads\\profile_avatar_29.jfif', 2);

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
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=40;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=42;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
