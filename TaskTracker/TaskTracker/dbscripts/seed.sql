--
-- PostgreSQL database dump
--

-- Dumped from database version 13.1
-- Dumped by pg_dump version 13.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE IF EXISTS postgres;
--
-- Name: postgres; Type: DATABASE; Schema: -; Owner: user
--

CREATE DATABASE postgres WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'C';


ALTER DATABASE postgres OWNER TO "user";

\connect postgres

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Projects; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Projects" (
    "Id" bigint NOT NULL,
    "Name" text NOT NULL,
    "StartDate" timestamp without time zone,
    "CompletionDate" timestamp without time zone,
    "Status" integer DEFAULT 0 NOT NULL,
    "Priority" integer NOT NULL
);


ALTER TABLE public."Projects" OWNER TO postgres;

--
-- Name: Projects_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Projects" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Projects_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Tasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Tasks" (
    "Id" bigint NOT NULL,
    "Name" text,
    "Status" integer NOT NULL,
    "Description" text,
    "Priority" integer NOT NULL,
    "ProjectId" bigint
);


ALTER TABLE public."Tasks" OWNER TO postgres;

--
-- Name: Tasks_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Tasks" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Tasks_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: Projects; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Projects" ("Id", "Name", "StartDate", "CompletionDate", "Status", "Priority") FROM stdin;
20	Name	2021-03-03 00:00:00	2021-04-03 00:00:00	2	1
19	Name	2021-03-03 00:00:00	2022-03-03 00:00:00	1	155
24	Really New Project	2021-03-03 00:00:00	2021-03-03 00:00:00	0	0
25	My Project	\N	\N	0	0
26	My Project	\N	\N	0	0
27	My Project	\N	\N	0	0
28	My Project	\N	\N	0	0
29	My Project	\N	\N	0	0
30	My Project	\N	\N	0	0
31	My Project	\N	\N	0	0
32	My Project	\N	\N	0	0
33	My Project	\N	\N	0	0
17	Another Project Name	\N	\N	0	0
\.


--
-- Data for Name: Tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Tasks" ("Id", "Name", "Status", "Description", "Priority", "ProjectId") FROM stdin;
4	New Taks	0	Some Description	12	19
5	New Taks	0	\N	0	19
6	\N	0	\N	0	19
7	To Do Something	0	\N	0	\N
8	To Do Something	0	\N	0	\N
9	To Do Something	0	\N	0	17
10	Some Name	0	\N	0	17
3	Task 2	0	adsfa	1	\N
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20210528124651_InitialCreate	5.0.6
20210601162405_ProjectDateCanBeNull	5.0.6
20210601183345_OnDeleteProjectIgnore	5.0.6
\.


--
-- Name: Projects_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Projects_Id_seq"', 33, true);


--
-- Name: Tasks_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Tasks_Id_seq"', 10, true);


--
-- Name: Projects PK_Projects; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Projects"
    ADD CONSTRAINT "PK_Projects" PRIMARY KEY ("Id");


--
-- Name: Tasks PK_Tasks; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tasks"
    ADD CONSTRAINT "PK_Tasks" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Tasks_ProjectId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Tasks_ProjectId" ON public."Tasks" USING btree ("ProjectId");


--
-- Name: Tasks FK_Tasks_Projects_ProjectId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tasks"
    ADD CONSTRAINT "FK_Tasks_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES public."Projects"("Id") ON DELETE SET NULL;


--
-- PostgreSQL database dump complete
--

