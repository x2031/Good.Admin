-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- 主机： 192.168.10.35
-- 生成日期： 2022-07-31 09:53:47
-- 服务器版本： 8.0.29
-- PHP 版本： 8.0.21

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- 数据库： `good.admin`
--

-- --------------------------------------------------------

--
-- 表的结构 `Base_Action`
--

CREATE TABLE `Base_Action` (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '主键',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `CreatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '创建人Id',
  `Deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `ParentId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '父级Id',
  `Type` int NOT NULL COMMENT '类型,菜单=0,页面=1,权限=2',
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '权限名/菜单名',
  `Url` mediumtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci COMMENT '菜单地址',
  `Value` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '权限值',
  `NeedAction` tinyint NOT NULL DEFAULT '0' COMMENT '是否需要权限(仅页面有效)',
  `Icon` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '图标',
  `Sort` int NOT NULL DEFAULT '0' COMMENT '排序'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='系统权限表';

--
-- 转存表中的数据 `Base_Action`
--

INSERT INTO `Base_Action` (`Id`, `CreateTime`, `CreatorId`, `Deleted`, `ParentId`, `Type`, `Name`, `Url`, `Value`, `NeedAction`, `Icon`, `Sort`) VALUES
('1178957405992521728', '2019-10-01 16:58:44', NULL, 0, NULL, 0, '系统管理', '', NULL, 1, 'setting', 1),
('1178957553778823168', '2019-10-01 16:59:19', NULL, 0, '1178957405992521728', 1, '权限管理', '/Base_Manage/Base_Action/List', NULL, 1, NULL, 20),
('1179018395304071168', '2019-10-01 21:01:05', NULL, 0, '1178957405992521728', 1, '密钥管理', '/Base_Manage/Base_AppSecret/List', NULL, 1, NULL, 15),
('1182652266117599232', '2019-10-11 21:40:47', NULL, 0, '1178957405992521728', 1, '用户管理', '/Base_Manage/Base_User/List', NULL, 1, NULL, 0),
('1182652367447789568', '2019-10-11 21:41:11', NULL, 0, '1178957405992521728', 1, '角色管理', '/Base_Manage/Base_Role/List', NULL, 1, NULL, 5),
('1182652433302556672', '2019-10-11 21:41:27', NULL, 0, '1178957405992521728', 1, '部门管理', '/Base_Manage/Base_Department/List', NULL, 1, NULL, 10),
('1188801057778569216', '2019-10-28 20:53:53', NULL, 0, '1182652367447789568', 2, '增', NULL, 'Base_Role.Add', 1, NULL, 0),
('1188801057778569217', '2019-10-28 20:53:53', NULL, 0, '1182652367447789568', 2, '改', NULL, 'Base_Role.Edit', 1, NULL, 0),
('1188801057778569218', '2019-10-28 20:53:53', NULL, 0, '1182652367447789568', 2, '删', NULL, 'Base_Role.Delete', 1, NULL, 0),
('1188801109783744512', '2019-10-28 20:54:06', NULL, 0, '1182652433302556672', 2, '增', NULL, 'Base_Department.Add', 1, NULL, 0),
('1188801109783744513', '2019-10-28 20:54:06', NULL, 0, '1182652433302556672', 2, '改', NULL, 'Base_Department.Edit', 1, NULL, 0),
('1188801109783744514', '2019-10-28 20:54:06', NULL, 0, '1182652433302556672', 2, '删', NULL, 'Base_Department.Delete', 1, NULL, 0),
('1188801273885888512', '2019-10-28 20:54:45', NULL, 0, '1179018395304071168', 2, '增', NULL, 'Base_AppSecret.Add', 1, NULL, 0),
('1188801273885888513', '2019-10-28 20:54:45', NULL, 0, '1179018395304071168', 2, '改', NULL, 'Base_AppSecret.Edit', 1, NULL, 0),
('1188801273885888514', '2019-10-28 20:54:45', NULL, 0, '1179018395304071168', 2, '删', NULL, 'Base_AppSecret.Delete', 1, NULL, 0),
('1188801341661646848', '2019-10-28 20:55:01', NULL, 0, '1178957553778823168', 2, '增', NULL, 'Base_Action.Add', 1, NULL, 0),
('1188801341661646849', '2019-10-28 20:55:01', NULL, 0, '1178957553778823168', 2, '改', NULL, 'Base_Action.Edit', 1, NULL, 0),
('1188801341661646850', '2019-10-28 20:55:01', NULL, 0, '1178957553778823168', 2, '删', NULL, 'Base_Action.Delete', 1, NULL, 0),
('1193158266167758848', '2019-11-09 21:27:53', 'Admin', 0, NULL, 0, '首页', NULL, NULL, 1, 'home', 0),
('1193158630615027712', '2019-11-09 21:29:20', 'Admin', 0, '1193158266167758848', 1, '框架介绍', '/Home/Introduce', NULL, 0, NULL, 0),
('1193158780011941888', '2019-11-09 21:29:55', 'Admin', 0, '1193158266167758848', 1, '运营统计', '/Home/Statis', NULL, 0, NULL, 0),
('1251792477787000832', '2020-04-19 16:39:19', NULL, 0, '1182652266117599232', 2, '增', NULL, 'Base_User.Add', 1, NULL, 0),
('1251792477787000833', '2020-04-19 16:39:19', NULL, 0, '1182652266117599232', 2, '改', NULL, 'Base_User.Edit', 1, NULL, 0),
('1251792477787000834', '2020-04-19 16:39:19', NULL, 0, '1182652266117599232', 2, '删', NULL, 'Base_User.Delete', 1, NULL, 0),
('1251792721769664512', '2020-04-19 16:40:17', 'Admin', 0, '1178957405992521728', 1, '操作日志', '/Base_Manage/Base_UserLog/List', NULL, 0, NULL, 23);

-- --------------------------------------------------------

--
-- 表的结构 `Base_AppSecret`
--

CREATE TABLE `Base_AppSecret` (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '自然主键',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `CreatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '创建人Id',
  `Deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `AppId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '应用Id',
  `AppSecret` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '应用密钥',
  `AppName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '应用名'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='应用密钥表';

--
-- 转存表中的数据 `Base_AppSecret`
--

INSERT INTO `Base_AppSecret` (`Id`, `CreateTime`, `CreatorId`, `Deleted`, `AppId`, `AppSecret`, `AppName`) VALUES
('1172497995938271232', '2019-09-13 21:11:20', 'Admin', 0, 'PcAdmin', 'wtMaiTRPTT3hrf5e', '后台AppId'),
('1173937877642383360', '2019-09-17 20:32:55', 'Admin', 0, 'AppAdmin', 'IVh9LLSVFcoQPQ5K', 'APP密钥');

-- --------------------------------------------------------

--
-- 表的结构 `Base_BuildTest`
--

CREATE TABLE `Base_BuildTest` (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '自然主键',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `CreatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '创建人Id',
  `Deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `Column1` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '列1',
  `Column2` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '列2',
  `Column3` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '列3',
  `Column4` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '列4',
  `Column5` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '列5'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='生成测试表';

-- --------------------------------------------------------

--
-- 表的结构 `Base_DbLink`
--

CREATE TABLE `Base_DbLink` (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '自然主键',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `CreatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '创建人Id',
  `Deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `LinkName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '连接名',
  `ConnectionStr` mediumtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci COMMENT '连接字符串',
  `DbType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '数据库类型'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='数据库连接表';

--
-- 转存表中的数据 `Base_DbLink`
--

INSERT INTO `Base_DbLink` (`Id`, `CreateTime`, `CreatorId`, `Deleted`, `LinkName`, `ConnectionStr`, `DbType`) VALUES
('1183373232498020352', '2019-10-13 21:25:39', 'Admin', 0, 'BaseDb', 'Data Source=.;Initial Catalog=Colder.Admin.AntdVue;Integrated Security=True', 'SqlServer');

-- --------------------------------------------------------

--
-- 表的结构 `Base_Department`
--

CREATE TABLE `Base_Department` (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '主键',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `CreatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '创建人Id',
  `Deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '部门名',
  `ParentId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '上级部门Id'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='部门表';

--
-- 转存表中的数据 `Base_Department`
--

INSERT INTO `Base_Department` (`Id`, `CreateTime`, `CreatorId`, `Deleted`, `Name`, `ParentId`) VALUES
('1181175685528424448', '2019-10-07 19:53:23', NULL, 0, '宁波分公司', NULL),
('1181175803631636480', '2019-10-07 19:53:51', NULL, 0, '鄞州事业部', '1181175685528424448'),
('1181175865409540096', '2019-10-07 19:54:06', NULL, 0, '海曙事业部', '1181175685528424448');

-- --------------------------------------------------------

--
-- 表的结构 `Base_Role`
--

CREATE TABLE `Base_Role` (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '主键',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `CreatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '创建人Id',
  `Deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `RoleName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '角色名'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='系统角色表';

--
-- 转存表中的数据 `Base_Role`
--

INSERT INTO `Base_Role` (`Id`, `CreateTime`, `CreatorId`, `Deleted`, `RoleName`) VALUES
('1180486275199668224', '2019-10-05 22:13:55', NULL, 0, '超级管理员'),
('1180819481383931904', '2019-10-06 20:17:57', NULL, 0, '部门管理员');

-- --------------------------------------------------------

--
-- 表的结构 `Base_RoleAction`
--

CREATE TABLE `Base_RoleAction` (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '主键',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `CreatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '创建人Id',
  `Deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `RoleId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '用户Id',
  `ActionId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '权限Id'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='角色权限表';

--
-- 转存表中的数据 `Base_RoleAction`
--

INSERT INTO `Base_RoleAction` (`Id`, `CreateTime`, `CreatorId`, `Deleted`, `RoleId`, `ActionId`) VALUES
('1188801858282459136', '2019-10-28 20:57:04', NULL, 0, '1180486275199668224', '1182654049414025216'),
('1188801858282459137', '2019-10-28 20:57:04', NULL, 0, '1180486275199668224', '1182654208411701248'),
('1188801858282459138', '2019-10-28 20:57:04', NULL, 0, '1180486275199668224', '1183370665412005888'),
('1188801984434540544', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1188044797802188800'),
('1188801984434540545', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1188044797802188801'),
('1188801984434540546', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1182652433302556672'),
('1188801984434540547', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1178957405992521728'),
('1188801984434540548', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1188801109783744512'),
('1188801984434540549', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1188801109783744513'),
('1188801984434540550', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1188801109783744514'),
('1188801984434540551', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1182652266117599232'),
('1188801984434540552', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1188800845714558976'),
('1188801984434540553', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1188800845714558977'),
('1188801984434540554', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1188800845714558978'),
('1188801984434540555', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1182652367447789568'),
('1188801984434540556', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1188801057778569216'),
('1188801984434540557', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1188801057778569217'),
('1188801984434540558', '2019-10-28 20:57:34', NULL, 0, '1180819481383931904', '1188801057778569218');

-- --------------------------------------------------------

--
-- 表的结构 `Base_User`
--

CREATE TABLE `Base_User` (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '主键',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `CreatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '创建人Id',
  `Deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `UserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '用户名',
  `Password` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '密码',
  `RealName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '姓名',
  `Sex` int NOT NULL DEFAULT '0' COMMENT '性别(1为男，0为女)',
  `Birthday` date DEFAULT NULL COMMENT '出生日期',
  `DepartmentId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '所属部门Id'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='系统用户表';

--
-- 转存表中的数据 `Base_User`
--

INSERT INTO `Base_User` (`Id`, `CreateTime`, `CreatorId`, `Deleted`, `UserName`, `Password`, `RealName`, `Sex`, `Birthday`, `DepartmentId`) VALUES
('1181928860648738816', '2019-10-09 21:46:14', NULL, 0, 'xiaohua', 'e10adc3949ba59abbe56e057f20f883e', '小花', 0, NULL, NULL),
('1183363221872971776', '2019-10-13 20:45:52', 'Admin', 0, 'aa', NULL, 'aaa', 0, NULL, NULL),
('Admin', '2019-09-13 21:10:03', 'Admin', 0, 'Admin', 'e10adc3949ba59abbe56e057f20f883e', '超级管理员', 1, '2019-09-13', NULL);

-- --------------------------------------------------------

--
-- 表的结构 `Base_UserLog`
--

CREATE TABLE `Base_UserLog` (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '自然主键',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `CreatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '创建人Id',
  `CreatorRealName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '创建人姓名',
  `LogType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '日志类型',
  `LogContent` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci COMMENT '日志内容'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='系统日志表';

-- --------------------------------------------------------

--
-- 表的结构 `Base_UserRole`
--

CREATE TABLE `Base_UserRole` (
  `Id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '主键',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `CreatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '创建人Id',
  `Deleted` tinyint NOT NULL DEFAULT '0' COMMENT '否已删除',
  `UserId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '用户Id',
  `RoleId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '角色Id'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='用户角色表';

--
-- 转存表中的数据 `Base_UserRole`
--

INSERT INTO `Base_UserRole` (`Id`, `CreateTime`, `CreatorId`, `Deleted`, `UserId`, `RoleId`) VALUES
('1181927367719784448', '2019-10-09 21:40:18', NULL, 0, '1181922344629702656', '1180819481383931904'),
('1181927367719784449', '2019-10-09 21:40:18', NULL, 0, '1181922344629702656', '1180486275199668224'),
('1181927783786352640', '2019-10-09 21:41:57', NULL, 0, '1181927783727632384', '1180819481383931904'),
('1251792318642524160', '2020-04-19 16:38:41', NULL, 0, '1181928860648738816', '1180819481383931904');

--
-- 转储表的索引
--

--
-- 表的索引 `Base_Action`
--
ALTER TABLE `Base_Action`
  ADD PRIMARY KEY (`Id`);

--
-- 表的索引 `Base_AppSecret`
--
ALTER TABLE `Base_AppSecret`
  ADD PRIMARY KEY (`Id`);

--
-- 表的索引 `Base_BuildTest`
--
ALTER TABLE `Base_BuildTest`
  ADD PRIMARY KEY (`Id`);

--
-- 表的索引 `Base_DbLink`
--
ALTER TABLE `Base_DbLink`
  ADD PRIMARY KEY (`Id`);

--
-- 表的索引 `Base_Department`
--
ALTER TABLE `Base_Department`
  ADD PRIMARY KEY (`Id`);

--
-- 表的索引 `Base_Role`
--
ALTER TABLE `Base_Role`
  ADD PRIMARY KEY (`Id`);

--
-- 表的索引 `Base_RoleAction`
--
ALTER TABLE `Base_RoleAction`
  ADD PRIMARY KEY (`Id`);

--
-- 表的索引 `Base_User`
--
ALTER TABLE `Base_User`
  ADD PRIMARY KEY (`Id`);

--
-- 表的索引 `Base_UserLog`
--
ALTER TABLE `Base_UserLog`
  ADD PRIMARY KEY (`Id`);

--
-- 表的索引 `Base_UserRole`
--
ALTER TABLE `Base_UserRole`
  ADD PRIMARY KEY (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
