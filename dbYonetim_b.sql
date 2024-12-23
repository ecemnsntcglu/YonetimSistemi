PGDMP  9    (                |         	   dbYonetim    14.13    16.4 G    ?           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            @           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            A           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            B           1262    16559 	   dbYonetim    DATABASE     �   CREATE DATABASE "dbYonetim" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Turkish_T�rkiye.1254';
    DROP DATABASE "dbYonetim";
                postgres    false                        2615    2200    public    SCHEMA     2   -- *not* creating schema, since initdb creates it
 2   -- *not* dropping schema, since initdb creates it
                postgres    false            C           0    0    SCHEMA public    ACL     Q   REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;
                   postgres    false    4            �            1255    16773    create_bilet_for_etkinlik()    FUNCTION     �  CREATE FUNCTION public.create_bilet_for_etkinlik() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Eğer etkinlik türü "tiyatro" ise mevcut mantık
    IF NEW.etkinlik_tur = 'konser' THEN
        INSERT INTO biletler (salon_no, koltuk_id, etkinlik_id, satis_tarihi, fiyat, durum)
        SELECT 
            k.salon_no,
            k.koltuk_id,
            NEW.etkinlik_id,
            CURRENT_DATE, -- Mevcut tarih
            CASE
                WHEN k.bolum = 'A' THEN 2500
                WHEN k.bolum = 'B' THEN 2000
                WHEN k.bolum = 'C' THEN 1500
                WHEN k.bolum IN ('D', 'E') THEN 1000
            END,
            FALSE -- durum (varsayılan olarak boş)
        FROM koltuk_duzeni k
        WHERE k.salon_no = NEW.salon_no;
    
    -- Eğer etkinlik türü "sinema" ise sadece A ve B bölümlerini kullan
    ELSIF NEW.etkinlik_turu = 'sinema'or NEW.etkinlik_turu = 'tiyatro'  THEN
        INSERT INTO biletler (salon_no, koltuk_id, etkinlik_id, satis_tarihi, fiyat, durum)
        SELECT 
            k.salon_no,
            k.koltuk_id,
            NEW.etkinlik_id,
            CURRENT_DATE, -- Mevcut tarih
            CASE
                WHEN k.bolum = 'A' THEN 500
                WHEN k.bolum = 'B' THEN 600
            END,
            FALSE -- durum (varsayılan olarak boş)
        FROM koltuk_duzeni k
        WHERE k.salon_no = NEW.salon_no
          AND k.bolum IN ('A', 'B'); -- Sadece A ve B bölümleri
    END IF;

    RETURN NEW;
END;
$$;
 2   DROP FUNCTION public.create_bilet_for_etkinlik();
       public          postgres    false    4            �            1255    16776    delete_biletler_for_etkinlik()    FUNCTION     �   CREATE FUNCTION public.delete_biletler_for_etkinlik() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM biletler WHERE etkinlik_id = OLD.etkinlik_id;
    RETURN OLD;
END;
$$;
 5   DROP FUNCTION public.delete_biletler_for_etkinlik();
       public          postgres    false    4            �            1255    16839    delete_past_events()    FUNCTION     �   CREATE FUNCTION public.delete_past_events() RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM etkinlikler WHERE tarih < CURRENT_DATE;
END;
$$;
 +   DROP FUNCTION public.delete_past_events();
       public          postgres    false    4            �            1255    16768 )   ensure_koltuk_id_sequence_exists(integer)    FUNCTION     �  CREATE FUNCTION public.ensure_koltuk_id_sequence_exists(salon_no integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
    sequence_name TEXT;
BEGIN
    sequence_name := 'salon' || salon_no || '_koltuk_id_seq';

    -- Sequence mevcut mu kontrol et
    IF NOT EXISTS (SELECT 1 FROM pg_class WHERE relkind = 'S' AND relname = sequence_name) THEN
        EXECUTE 'CREATE SEQUENCE ' || sequence_name || ' START 1';
    END IF;
END;
$$;
 I   DROP FUNCTION public.ensure_koltuk_id_sequence_exists(salon_no integer);
       public          postgres    false    4            �            1255    16837    get_bilet_sayisi(integer)    FUNCTION       CREATE FUNCTION public.get_bilet_sayisi(etkinlik_id integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
    bilet_sayisi INT;
BEGIN
    SELECT COUNT(*) INTO bilet_sayisi FROM biletler WHERE etkinlik_id = etkinlik_id;
    RETURN bilet_sayisi;
END;
$$;
 <   DROP FUNCTION public.get_bilet_sayisi(etkinlik_id integer);
       public          postgres    false    4            �            1255    16771 6   koltuk_olustur(character, integer, character, integer)    FUNCTION     �  CREATE FUNCTION public.koltuk_olustur(ust_bolum character, koltuk_sayisi1 integer, son_bolum character, koltuk_sayisi2 integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
DECLARE
    
    bolum CHAR(1);
    sira CHAR(1);
    koltuk_no INT;
    koltuk_id INT; -- Koltuk ID başlangıcı
    sira_sayisi INT; -- Sıra sayısı
BEGIN
    -- Her salon için döngü
   
        koltuk_id := 1; -- Koltuk ID'yi her salon için sıfırla

        -- Bölüm A'dan üst_bolum'a kadar 60 koltukluk düzen
        FOR bolum IN SELECT chr(g) FROM generate_series(ASCII('A'), ASCII(ust_bolum)) AS g LOOP
            sira_sayisi := koltuk_sayisi1 / 15; -- 15 koltuk/sıra varsayarak sıra sayısını hesapla
            FOR sira IN SELECT chr(h) FROM generate_series(ASCII('A'), ASCII('A') + sira_sayisi - 1) AS h LOOP
                FOR koltuk_no IN 1..15 LOOP
                    INSERT INTO koltuk_duzeni (koltuk_id, salon_no, bolum, sira_no, koltuk_no)
                    VALUES (koltuk_id, salon.salon_no, bolum, sira, koltuk_no);
                    koltuk_id := koltuk_id + 1; -- Koltuk ID'yi artır
                END LOOP;
            END LOOP;
        END LOOP;

        -- Bölüm (üst_bolum + 1)'den son_bolum'a kadar 90 koltukluk düzen
        FOR bolum IN SELECT chr(g) FROM generate_series(ASCII(ust_bolum) + 1, ASCII(son_bolum)) AS g LOOP
            sira_sayisi := koltuk_sayisi2 / 15; -- 15 koltuk/sıra varsayarak sıra sayısını hesapla
            FOR sira IN SELECT chr(h) FROM generate_series(ASCII('A'), ASCII('A') + sira_sayisi - 1) AS h LOOP
                FOR koltuk_no IN 1..15 LOOP
                    INSERT INTO koltuk_duzeni (koltuk_id, salon_no, bolum, sira_no, koltuk_no)
                    VALUES (koltuk_id, salon.salon_no, bolum, sira, koltuk_no);
                    koltuk_id := koltuk_id + 1; -- Koltuk ID'yi artır
                END LOOP;
            END LOOP;
        END LOOP;

   
END;
$$;
    DROP FUNCTION public.koltuk_olustur(ust_bolum character, koltuk_sayisi1 integer, son_bolum character, koltuk_sayisi2 integer);
       public          postgres    false    4            �            1259    16561    adminler    TABLE     �   CREATE TABLE public.adminler (
    admin_id integer NOT NULL,
    username character varying(50) NOT NULL,
    password character varying(100) NOT NULL
);
    DROP TABLE public.adminler;
       public         heap    postgres    false    4            �            1259    16560    adminler_admin_id_seq    SEQUENCE     �   CREATE SEQUENCE public.adminler_admin_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.adminler_admin_id_seq;
       public          postgres    false    210    4            D           0    0    adminler_admin_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.adminler_admin_id_seq OWNED BY public.adminler.admin_id;
          public          postgres    false    209            �            1259    16634    biletler    TABLE     <  CREATE TABLE public.biletler (
    bilet_id integer NOT NULL,
    kullanici_id integer,
    etkinlik_id integer NOT NULL,
    fiyat numeric DEFAULT 1000 NOT NULL,
    satis_tarihi timestamp without time zone,
    salon_no integer NOT NULL,
    koltuk_id integer NOT NULL,
    durum boolean DEFAULT false NOT NULL
);
    DROP TABLE public.biletler;
       public         heap    postgres    false    4            �            1259    16633    biletler_bilet_id_seq    SEQUENCE     �   CREATE SEQUENCE public.biletler_bilet_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.biletler_bilet_id_seq;
       public          postgres    false    4    216            E           0    0    biletler_bilet_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.biletler_bilet_id_seq OWNED BY public.biletler.bilet_id;
          public          postgres    false    215            �            1259    16670    etkinlikler    TABLE     �   CREATE TABLE public.etkinlikler (
    etkinlik_id integer NOT NULL,
    etkinlik_tur character varying(50),
    ad character varying(100),
    tarih date,
    salon_no integer,
    resim_yolu character varying(255),
    sehir character varying(10)
);
    DROP TABLE public.etkinlikler;
       public         heap    postgres    false    4            �            1259    16669    etkinlikler_id_seq    SEQUENCE     �   CREATE SEQUENCE public.etkinlikler_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public.etkinlikler_id_seq;
       public          postgres    false    4    218            F           0    0    etkinlikler_id_seq    SEQUENCE OWNED BY     R   ALTER SEQUENCE public.etkinlikler_id_seq OWNED BY public.etkinlikler.etkinlik_id;
          public          postgres    false    217            �            1259    16702    koltuk_duzeni    TABLE     �   CREATE TABLE public.koltuk_duzeni (
    koltuk_id integer NOT NULL,
    salon_no integer NOT NULL,
    bolum character varying NOT NULL,
    sira_no character varying NOT NULL,
    koltuk_no integer NOT NULL
);
 !   DROP TABLE public.koltuk_duzeni;
       public         heap    postgres    false    4            �            1259    16701    koltuk_duzeni_koltuk_id_seq    SEQUENCE     �   CREATE SEQUENCE public.koltuk_duzeni_koltuk_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 2   DROP SEQUENCE public.koltuk_duzeni_koltuk_id_seq;
       public          postgres    false    220    4            G           0    0    koltuk_duzeni_koltuk_id_seq    SEQUENCE OWNED BY     [   ALTER SEQUENCE public.koltuk_duzeni_koltuk_id_seq OWNED BY public.koltuk_duzeni.koltuk_id;
          public          postgres    false    219            �            1259    16568    kullanicilar    TABLE       CREATE TABLE public.kullanicilar (
    kullanici_id integer NOT NULL,
    username character varying(50) NOT NULL,
    password character varying(100) NOT NULL,
    ad character varying(50) NOT NULL,
    soyad character varying(50) NOT NULL,
    tel character varying(20) NOT NULL,
    email character varying(50) NOT NULL,
    CONSTRAINT chk_tel_format CHECK (((tel)::text ~ '^[0-9]{10}$'::text)),
    CONSTRAINT email_format_check CHECK (((email)::text ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$'::text))
);
     DROP TABLE public.kullanicilar;
       public         heap    postgres    false    4            �            1259    16567    kullanicilar_kullanici_id_seq    SEQUENCE     �   CREATE SEQUENCE public.kullanicilar_kullanici_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 4   DROP SEQUENCE public.kullanicilar_kullanici_id_seq;
       public          postgres    false    4    212            H           0    0    kullanicilar_kullanici_id_seq    SEQUENCE OWNED BY     _   ALTER SEQUENCE public.kullanicilar_kullanici_id_seq OWNED BY public.kullanicilar.kullanici_id;
          public          postgres    false    211            �            1259    16762    salon1_koltuk_id_seq    SEQUENCE     }   CREATE SEQUENCE public.salon1_koltuk_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public.salon1_koltuk_id_seq;
       public          postgres    false    4            �            1259    16763    salon2_koltuk_id_seq    SEQUENCE     }   CREATE SEQUENCE public.salon2_koltuk_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public.salon2_koltuk_id_seq;
       public          postgres    false    4            �            1259    16769    salon3_koltuk_id_seq    SEQUENCE     }   CREATE SEQUENCE public.salon3_koltuk_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public.salon3_koltuk_id_seq;
       public          postgres    false    4            �            1259    16575    salonlar    TABLE     �   CREATE TABLE public.salonlar (
    salon_no integer NOT NULL,
    salon_ad character varying(100) NOT NULL,
    kapasite integer NOT NULL,
    sehir character varying(20) NOT NULL
);
    DROP TABLE public.salonlar;
       public         heap    postgres    false    4            �            1259    16574    salonlar_salon_no_seq    SEQUENCE     �   CREATE SEQUENCE public.salonlar_salon_no_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.salonlar_salon_no_seq;
       public          postgres    false    214    4            I           0    0    salonlar_salon_no_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.salonlar_salon_no_seq OWNED BY public.salonlar.salon_no;
          public          postgres    false    213            ~           2604    16564    adminler admin_id    DEFAULT     v   ALTER TABLE ONLY public.adminler ALTER COLUMN admin_id SET DEFAULT nextval('public.adminler_admin_id_seq'::regclass);
 @   ALTER TABLE public.adminler ALTER COLUMN admin_id DROP DEFAULT;
       public          postgres    false    210    209    210            �           2604    16637    biletler bilet_id    DEFAULT     v   ALTER TABLE ONLY public.biletler ALTER COLUMN bilet_id SET DEFAULT nextval('public.biletler_bilet_id_seq'::regclass);
 @   ALTER TABLE public.biletler ALTER COLUMN bilet_id DROP DEFAULT;
       public          postgres    false    215    216    216            �           2604    16673    etkinlikler etkinlik_id    DEFAULT     y   ALTER TABLE ONLY public.etkinlikler ALTER COLUMN etkinlik_id SET DEFAULT nextval('public.etkinlikler_id_seq'::regclass);
 F   ALTER TABLE public.etkinlikler ALTER COLUMN etkinlik_id DROP DEFAULT;
       public          postgres    false    217    218    218                       2604    16571    kullanicilar kullanici_id    DEFAULT     �   ALTER TABLE ONLY public.kullanicilar ALTER COLUMN kullanici_id SET DEFAULT nextval('public.kullanicilar_kullanici_id_seq'::regclass);
 H   ALTER TABLE public.kullanicilar ALTER COLUMN kullanici_id DROP DEFAULT;
       public          postgres    false    211    212    212            �           2604    16766    salonlar salon_no    DEFAULT     v   ALTER TABLE ONLY public.salonlar ALTER COLUMN salon_no SET DEFAULT nextval('public.salonlar_salon_no_seq'::regclass);
 @   ALTER TABLE public.salonlar ALTER COLUMN salon_no DROP DEFAULT;
       public          postgres    false    214    213    214            /          0    16561    adminler 
   TABLE DATA           @   COPY public.adminler (admin_id, username, password) FROM stdin;
    public          postgres    false    210   e       5          0    16634    biletler 
   TABLE DATA           x   COPY public.biletler (bilet_id, kullanici_id, etkinlik_id, fiyat, satis_tarihi, salon_no, koltuk_id, durum) FROM stdin;
    public          postgres    false    216   <e       7          0    16670    etkinlikler 
   TABLE DATA           h   COPY public.etkinlikler (etkinlik_id, etkinlik_tur, ad, tarih, salon_no, resim_yolu, sehir) FROM stdin;
    public          postgres    false    218   �|       9          0    16702    koltuk_duzeni 
   TABLE DATA           W   COPY public.koltuk_duzeni (koltuk_id, salon_no, bolum, sira_no, koltuk_no) FROM stdin;
    public          postgres    false    220   �}       1          0    16568    kullanicilar 
   TABLE DATA           _   COPY public.kullanicilar (kullanici_id, username, password, ad, soyad, tel, email) FROM stdin;
    public          postgres    false    212   ��       3          0    16575    salonlar 
   TABLE DATA           G   COPY public.salonlar (salon_no, salon_ad, kapasite, sehir) FROM stdin;
    public          postgres    false    214   4�       J           0    0    adminler_admin_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.adminler_admin_id_seq', 1, true);
          public          postgres    false    209            K           0    0    biletler_bilet_id_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public.biletler_bilet_id_seq', 2043, true);
          public          postgres    false    215            L           0    0    etkinlikler_id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public.etkinlikler_id_seq', 8, true);
          public          postgres    false    217            M           0    0    koltuk_duzeni_koltuk_id_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public.koltuk_duzeni_koltuk_id_seq', 1, false);
          public          postgres    false    219            N           0    0    kullanicilar_kullanici_id_seq    SEQUENCE SET     K   SELECT pg_catalog.setval('public.kullanicilar_kullanici_id_seq', 6, true);
          public          postgres    false    211            O           0    0    salon1_koltuk_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.salon1_koltuk_id_seq', 782, true);
          public          postgres    false    221            P           0    0    salon2_koltuk_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.salon2_koltuk_id_seq', 780, true);
          public          postgres    false    222            Q           0    0    salon3_koltuk_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.salon3_koltuk_id_seq', 780, true);
          public          postgres    false    223            R           0    0    salonlar_salon_no_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public.salonlar_salon_no_seq', 3, true);
          public          postgres    false    213            �           2606    16566    adminler adminler_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.adminler
    ADD CONSTRAINT adminler_pkey PRIMARY KEY (admin_id);
 @   ALTER TABLE ONLY public.adminler DROP CONSTRAINT adminler_pkey;
       public            postgres    false    210            �           2606    16639    biletler biletler_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.biletler
    ADD CONSTRAINT biletler_pkey PRIMARY KEY (bilet_id);
 @   ALTER TABLE ONLY public.biletler DROP CONSTRAINT biletler_pkey;
       public            postgres    false    216            �           2606    16675    etkinlikler etkinlikler_pkey 
   CONSTRAINT     c   ALTER TABLE ONLY public.etkinlikler
    ADD CONSTRAINT etkinlikler_pkey PRIMARY KEY (etkinlik_id);
 F   ALTER TABLE ONLY public.etkinlikler DROP CONSTRAINT etkinlikler_pkey;
       public            postgres    false    218            �           2606    16747     koltuk_duzeni koltuk_duzeni_pkey 
   CONSTRAINT     o   ALTER TABLE ONLY public.koltuk_duzeni
    ADD CONSTRAINT koltuk_duzeni_pkey PRIMARY KEY (salon_no, koltuk_id);
 J   ALTER TABLE ONLY public.koltuk_duzeni DROP CONSTRAINT koltuk_duzeni_pkey;
       public            postgres    false    220    220            �           2606    16573    kullanicilar kullanicilar_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.kullanicilar
    ADD CONSTRAINT kullanicilar_pkey PRIMARY KEY (kullanici_id);
 H   ALTER TABLE ONLY public.kullanicilar DROP CONSTRAINT kullanicilar_pkey;
       public            postgres    false    212            �           2606    16580    salonlar salonlar_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.salonlar
    ADD CONSTRAINT salonlar_pkey PRIMARY KEY (salon_no);
 @   ALTER TABLE ONLY public.salonlar DROP CONSTRAINT salonlar_pkey;
       public            postgres    false    214            �           2606    16755 !   koltuk_duzeni unique_salon_koltuk 
   CONSTRAINT     k   ALTER TABLE ONLY public.koltuk_duzeni
    ADD CONSTRAINT unique_salon_koltuk UNIQUE (salon_no, koltuk_id);
 K   ALTER TABLE ONLY public.koltuk_duzeni DROP CONSTRAINT unique_salon_koltuk;
       public            postgres    false    220    220            �           2606    16830    kullanicilar uq_email 
   CONSTRAINT     Q   ALTER TABLE ONLY public.kullanicilar
    ADD CONSTRAINT uq_email UNIQUE (email);
 ?   ALTER TABLE ONLY public.kullanicilar DROP CONSTRAINT uq_email;
       public            postgres    false    212            �           2606    16832    kullanicilar uq_tel 
   CONSTRAINT     M   ALTER TABLE ONLY public.kullanicilar
    ADD CONSTRAINT uq_tel UNIQUE (tel);
 =   ALTER TABLE ONLY public.kullanicilar DROP CONSTRAINT uq_tel;
       public            postgres    false    212            �           2606    16836    kullanicilar uq_username 
   CONSTRAINT     W   ALTER TABLE ONLY public.kullanicilar
    ADD CONSTRAINT uq_username UNIQUE (username);
 B   ALTER TABLE ONLY public.kullanicilar DROP CONSTRAINT uq_username;
       public            postgres    false    212            �           2620    16777 !   etkinlikler after_etkinlik_delete    TRIGGER     �   CREATE TRIGGER after_etkinlik_delete AFTER DELETE ON public.etkinlikler FOR EACH ROW EXECUTE FUNCTION public.delete_biletler_for_etkinlik();
 :   DROP TRIGGER after_etkinlik_delete ON public.etkinlikler;
       public          postgres    false    235    218            �           2620    16775 !   etkinlikler after_etkinlik_insert    TRIGGER     �   CREATE TRIGGER after_etkinlik_insert AFTER INSERT ON public.etkinlikler FOR EACH ROW EXECUTE FUNCTION public.create_bilet_for_etkinlik();
 :   DROP TRIGGER after_etkinlik_insert ON public.etkinlikler;
       public          postgres    false    218    238            �           2606    16640 #   biletler biletler_kullanici_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.biletler
    ADD CONSTRAINT biletler_kullanici_id_fkey FOREIGN KEY (kullanici_id) REFERENCES public.kullanicilar(kullanici_id);
 M   ALTER TABLE ONLY public.biletler DROP CONSTRAINT biletler_kullanici_id_fkey;
       public          postgres    false    212    3210    216            �           2606    16819    biletler fk_etkinlik_id    FK CONSTRAINT     �   ALTER TABLE ONLY public.biletler
    ADD CONSTRAINT fk_etkinlik_id FOREIGN KEY (etkinlik_id) REFERENCES public.etkinlikler(etkinlik_id);
 A   ALTER TABLE ONLY public.biletler DROP CONSTRAINT fk_etkinlik_id;
       public          postgres    false    218    216    3222            �           2606    16824    biletler fk_koltuk_id    FK CONSTRAINT     �   ALTER TABLE ONLY public.biletler
    ADD CONSTRAINT fk_koltuk_id FOREIGN KEY (koltuk_id, salon_no) REFERENCES public.koltuk_duzeni(koltuk_id, salon_no);
 ?   ALTER TABLE ONLY public.biletler DROP CONSTRAINT fk_koltuk_id;
       public          postgres    false    216    220    3224    220    216            �           2606    16756    biletler fk_salon_no    FK CONSTRAINT     �   ALTER TABLE ONLY public.biletler
    ADD CONSTRAINT fk_salon_no FOREIGN KEY (salon_no, koltuk_id) REFERENCES public.koltuk_duzeni(salon_no, koltuk_id);
 >   ALTER TABLE ONLY public.biletler DROP CONSTRAINT fk_salon_no;
       public          postgres    false    216    220    220    3224    216            �           2606    16814    etkinlikler fk_salon_no    FK CONSTRAINT     �   ALTER TABLE ONLY public.etkinlikler
    ADD CONSTRAINT fk_salon_no FOREIGN KEY (salon_no) REFERENCES public.salonlar(salon_no);
 A   ALTER TABLE ONLY public.etkinlikler DROP CONSTRAINT fk_salon_no;
       public          postgres    false    214    3218    218            �           2606    16710 )   koltuk_duzeni koltuk_duzeni_salon_no_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.koltuk_duzeni
    ADD CONSTRAINT koltuk_duzeni_salon_no_fkey FOREIGN KEY (salon_no) REFERENCES public.salonlar(salon_no);
 S   ALTER TABLE ONLY public.koltuk_duzeni DROP CONSTRAINT koltuk_duzeni_salon_no_fkey;
       public          postgres    false    220    214    3218            /      x�3��,J��4426����� $�m      5      x���MG���5�}�n����!�����1��"1@L7�v4� =�x%zZٟ���?�����9ϱ�����<�����?����˪֝Ϯ�]���g���d���κ������̐�3f���a�C�a�C��"�һ������Ü��"5�Fz���yf����.s�w��3�e��p�9>�]挌�2gdd�9##���etdd�ё�]FGFv�etdd�ё�]FG��2:2g�ё9�����eld�.c#sv����Ȝ]�F��262g����]�FFw�eldt���]�GFw�e|dt��Q��#������2>2�����.�#c�L���2�߇#c�G�b���>�}82��pd�ه#�G��>�u�#�L���292�����.�#�L���292�����.�#�L�L�252�����.S#�L�L�252�����.S#��L�L�252�����.�#��L�L�2=2�����.�#��L�L�2=2�����.�#S�L�L�2�M�4�M�6�N�8�N�:�O�<�O�>�P�@�P�B�Q�D�QF��	��ބ�o�h��7a4I��0�&�MM��0�*a4Y �h�@<�ф�xÀ|����3� �:�$�6�"�>�&�c�����}�s9Fo �\��[���B�i!��D��h*Aa4� �0�NBM(�CM)�CM*�CM+�CM,�CM-�CM.�CM/�CM0�CM1�CM2%���FB	�����d�P�h�A(a4� �0�rJM:%���F���a���0�h�Aa4!�0��FMB#��!�F���a��d�p�h:B8a4!!�0��NMJ'��%�F�	��	���p�hzB8a4A!�0��AMR� ��)DF"��
��d��h�Ba4a!�0��AMZ�$��-DF"	������H�h�B$a4�!�0��IMb�$��1DF���Q��d�(�h:C�n�{:�r7:��v�3�!j7:��v�3�!j7:��v�3�!z7:��w�3�!z7:��	���v��k�Lg��3|.�����1z;��r������3|.�1��!�9K!�5�C,{��/�3�CMgȇ0�ΐa4�!�h:C>��t�|��)��t��h:C
a4�!�0�ΐBMgH!��3�F�R��)��t��h:C�h:C�h:C�h:C�h:C�h:C�h:C�h:C�h:C�h:C�h:C*a4�!�0�ΐJMgH%��3�F�R	�����t�T�h:C*a4�!�0�ΐFMgH#��3�F����i��t�4�h:Ca4�!�0�ΐFMgH#��3�F��	����t�t�h:C:a4�!�0�ΐNMgH'��3�F��	����t��h:Ca4�!�0�ΐAMg� ��3dF�2����t��h:C&a4�!�0�ΐIMg�$��3dF�2	�����t�L�h:C&a4�!�0�ΐEMg�"��3dF��v#}ƨv#�ΐ��t���H�3d�F:�!k7��Y��Ng�ލt:C�n���w#�ΐMMgH�3�t�$:�NgH�3�t�$:�NgH�3�t�$:�Ng��0��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg��Et��PDg�����?��7������ؿ������M�{����u�k�u�k����/���oX;�[��௖���0֡b���0�u��:��/��ab(�0<�1��O��%A��m8��tr����#���G T;�@�v!Q�D�ڍ�j7:0*���nt`T�сQ�FF��nt`ԻсQ�FF�)�z7R�n�0j�'9�n�0��HaԻ�<��f�.��hѢ�n4�h�g7�G���#Z�ٍ�-��F�}����<�E���`$���Hv#���F#ٍF�9�d7r�n�0���a$�/`$�����F��9��n�0:�Q��F�5��Y���F'�%�N�K�ڗ0:�/a�Ͼ��ʾ���}	#�u�0��(a��Q�Hw����F	#ݍF�%�l7J�n�0��(ad�Q��v����F#ۍ
F��l7*�nT0�ݨ`�Q��w����F#ߍF�5�|7j�n�0�ݨa�Q�(v��Q�F�؍F���ؑp~@4v%���	Dcw��ءp�@4v)� ͝
GDs���ܱp�@4	-$MB�A��Bt�$�P4	-dMB�A��Bx�"�P�-�-B�A��B|�7>��7~�{�@���Sh���s
��?|N����)���9�֛ >��z��Zo���B�_SdmBB��B��&�P"�	-�mB-B��B����B����B����B����B����B����B����B����B����B����B�0!��%L-�	Be�B�0!��&L-�	Bu�B�0!��'L-
;�
�B����Fa��B��Ch�R�!��)�Z�v-�
;�J�B��B�0%�+L	-�
SB��B�0%�,L	-SB��B�0%�-�-T3B��B�0#�.�-�3B��B�0#�/�-�3B��B�0'�0�	-sB	Ü�B�0'�1�	-TsBÜ�B�0'�2,-�B)Â�B˰صp��7ݵp��7ݵZ�Ů�#�鮅+�鮅3�鮅;�_��Z8t��Z�t��Z8u�Zh������8�;�M��B�m�Sh�-�s
��e|N�����)�ޖ�9���2>��z[��Zo����eXZhV�Z����aEh�eXZhV�Z�5���aMh�eXZhքZ�5���aMh�eXZhքZ�5���aMh�e����>ML	-�-�-�-�-�-�-�-�-�B-Å�B�p!��2\-�B-Å�B�p!��2\-�B-Å�B��Ch�e�!��2�Zh~-�?�Z�B-������B��Ch�e�!��2\	-�WB-Õ�B�p%��2\	-�WB-Õ�B�p%��2\	-�WB-Í�B�p#��2�-�7B-Í�B�p#��2�-�7B-Í�B�p#��2�	-�wB-Ý�B�p'��2�	-�wB-Ý�B�p'��2�	-�wB-Ã�B�� ��2<-��]W�c���صpGA<v-R�]��c��)�صpKA<w-S�]��s��9�$��2�h8� N�\T'ZN*�-7ĉ���
�D��Uq�eଂ8�2pWA�h8� N�\V'ZN+�-�ĉ���
�D��uq�e༂8�2p_A�h8� N�\X'ZN,�-7ĉ��#�D���q�e�̂8�2pgA�h8� N�\Z��	�e��ZhA�[� Z�-H-�$���{D���	�e���2prA�h�� A�]� Z�.H-g$����D���	�e���2pzA�h�� A�_� Z�/H-�$����D��	�e���2p�A�h�� A�a� Z�0H-g$���;D��!	�e���2p�A�h�� A�c� Z�1H-�$���{D��A	�e�"��2p�A�h�� A�e� Z�2H-g$����D��a	�e�2��2p�A�h�� A�g� Z�3H-�$����r?�R��7����*�"��|!���9�*��Jh�C�`|!�~�9����:h�Cؠ\|�	M4������� �  �%tb��3�~��{	��}���k�^B(w!<���Z��0�������%����F���{���Q�Fx���|/aT�^��{-�K�n�w��^��O30��������%���IF���{���Q�F��ۻ�}��nt�����n�F��ۻ�}��nt���ݗn�$�Q�F��ۻ���{-���r7�#�~��{�X�Fx���|/�������b��y{���t,w#<n��Z��0��O�����%�d7���~��|��������ľ��侄�Ծ������y�%���K��/att]�9{����ٍ��k�^���Fx���|/atv#<d��Z��0��������%�t7�#�~��{	#ݍ����k�^�Hw#<`��Z��0��������%�l7���~��{	#ۍ�t��k�^��v#�K�����%�l7¿���k�^��v#������%�|7¿~��k�^��w#y��;ҽ���+ݫ�;ӽ���;ݫ�;Խ���Kݫ;ս���[ݫ;ֽ��Ah!9dZh��C���Ah!;dZ���C&���Ih!=dZh���C��A����{!���9�֛>��z���Zo���B�-�Sh�	�s
��A|N��F��)��
�5E��"��!�-��,B%"��B��"��"�-ĈlB5"��B��&��#�	-�lBE"��B��&��$�	-D�lBU"��B���wY��)��0Q��2Q��4Q��6Q��8Q��:Q��<Q��>Q��@QBh�P�ZH%�E	��HQBh�R�Z�%�:E	��PQBh�T�ZHu-��:�bEB����\Q��B��Ch!X�!�P,�ZHu-4�:��E)��jQJh![�Z����E)��rQJh!]�Zh���E)��zQJh!_�Z�e�F���QFh!a�Zhe�"F���QFh!c�Z�e�BF9���QNh!e�Zh�ֽ�P�kݫ��A�(ߵ�U��]�^E(ߵ�U��]�^E�ص�U��]�^E�ص�U�
B-�ޖ!H߫����)�ޖ�9���2>��z[��Zo���B�m�Sh�-�s
��e|N�����)ZF%���QIh�eTZh��ZF%���QIh�eTZhU�ZF���QEh�eTZhU�ZF���QEh�eTZhU�ZF5���QMh�eTZhՄZF5���QMh�eTZhՄZF5���QMh�e�Ch�e�Ch�e�Ch�e�Ch�e�Ch�e�Ch�e�Ch�e�Ch�e�Ch�e�Ch�e�Zh-�ZF����Bh�e�Zh-�ZF����Bh�e�Zh-�ZFB-����ч�B��Ch�e�!��2�Zh}-��>�ZFB-�����Jh�e�Zh��ZF+����Jh�e�Zh��ZF+����Jh�e�Zhm�ZF����Fh�e�Zhm�ZF����Fh�e�Zhm�ZF����Nh�e�Zh�ZF��u�"��Z�*B��u�"��Z�*B��u�"��Z�*B��u�"t�Z�*BǮu�"t�Z�*B����D˸W�h�*B-�^Eh�eܫM��{���q�"4�2�U�&Zƽ��D˸W�h�*B-�^Eh�eܫM��{���q�"4�2�U�&Zƽ��D˸W�h�*B-�^Eh�eܫM��{���q�"4�2�U�&Zƽ��D˸W�h�*B-�^Eh�eܫM��{���q�"4�2�U�&Zƽ��D˸W�h�*B-�^Eh�e�*��ga�41%��e���ִ�ߔК��Z�2~SBkZ�oJhM��M	�i�)�5-�7%�ZD��U�ߔ�rh-W~SB+�E�\E�M	���2p�7%�ZD��U�ߔ�
h-W~?Fh������#�ZD��U�ߏZ	-�e�*������2p��c�VB�h���Z	-�e�*�oJh������)�U�"Z�"���VA�h���Z-�e�*�oJh������)�5-���_����=��      7   �   x���1�0��=(�?h�[���:���ژ� ��3x�1���[��ɗ�J5FjR˵��Z�$�1r��b���D�m���?XDo�n���b��:����� &��M%����.d�R?�y��p�s��ӹ�O��R�H�xܾ:��~��:Ɣ�P�}�      9   �  x�E�K��F��Ì���.GݭS���1 ��2m<x&��U���>���eHc��\vi_�XN�\.�Zn�^�Y����uٶ�mٺh��U�\�.��e��X6
����%��-��#��X�c�x.c��2�x/c�e]�.㭋%�SG��э1uveL�t���9���9Ź�]��C��S<�y��2o�^�#>˾vź�oݶ��S�.��{7�Խ+c�N�oL�/1.o11�ۯ��x.������,���r�u�rt_L=�0��SϮ��gv�S��s�c9�8�s�������k9o�^�G|�k�u�޺m��/�^]S�n��WW�ԋ���^������/�*˽��r1����ܻ�,���r�u�rw_L��0���S������Y}�8�g�syv1�1�N�\�K����x�|��'`}��+�X�4_��k�X�8߁����n�%��-��#���q�m�<�+x�w�.����|���h޺9�o]�˷����sy�E?�	��1����V��O}��z�~�C��{�S��ޣ��o����x���>�[�G?������η8�g7���չ|vw.�]��'����K��[��G��}��v��X�9��S���.����X���Ѽws.߻������K��s�A{~��c�q~L9��]��C��S���%���c��ȱ�\�+��oq4�ݜ�Ϯ��gw�����;��s~^r��r^?r_���&��kȱ��r,�v9�_Gw���-οj�9�_]�˯���w�����\~9���r\߻�Ǉ�����K���-����c��vW,��h~�9�?]�˟���O����s�s�y~�y���=c��ƺ��Mcr|�X��5�]�/��]�mc}�������}c���±vw,[�o�5'��W/�o�-�^��z[F�l����z�~�m�����2�]���eԳ�[o˨W��ޖQ��/o�oq4�n�士s���\>�<���m��������x�r��M��s���nʱ|�r,�Gw���G���\>�:�����{����|[�>�8ߧ��.��!��)����c�~��������]��x�����\~tu.?�;�]����m�%��-��#���q{nr,?���)��s�c�ytW~�~�����\~vu.?�;�_]��/��mא���r\_��Ǉ���˯K���-���c��vW,���h���~tu.��;��]��o��m�%��-��#���q�lr,�˟)��g�c�stW,��h~�9�?]]?f��~�ty������ޖY/���e���e���Woˬ���ޖY�_�-�޵�z[f=k���z���m�������-�歛c�ܺ:�ϭ�s����C��|[�v�y~�y��q<V9n�&��1�X>���.��qtW,oq���\>�:������s���~C�!��)����<>�=�X>/9��[���c��vW,���h޻9��]������{����|[�~�y~�y���x�����X~9�S���.����X~���|ts.?�:�ݝ��.��'����s�q~N9��]��C��S���%���c��ȱ�Z�+�_oq4_ݜ˯���Ww���s�E{�-��<��~�8�W9n�M�����;L9�߻�b��G��͹���\~ww.�<�q}槽��	J��w��;A靠�NPz'���ty'���\�I.�$�w��;�����N`y'��X�	,��w�;�坬(�$�w��;����Nry��y�X�	,��w�;����N`y'������Nry'���\�I.�4?�w�;����N`y'��X�	,��w����\�I.�$�w��;�����N`y'��X�	,��w�;����NV�w��;����Nry'������	,��w�;����N`y'��X�Ɋ�Nry'���\�I.�$�w���;����N`y'��X�	,��w�Nv��x'�ɀw2��x���z'��`��w��N0�	�;�x'��N���d�;�N���|��w��N0�	�;�x'��`��]x'�ɀw2��x'�)C{'��`��w��N0�	�;�x'��N���d�;�N�S��N0�	�;�x'��`��w��Nv�x'�ɀw2��x��`��w��N0�	�;�x'����;�N���d�;�N�;�x'��`��w��N0�	�;مw2��x'�ɀw2��2�w��N0�	�;�x'��`��w��d�;�N���d�;���;�x'��`��w��N0�	�;مw2��x'�ɀw2��2�w��N0�	�;�x'��`��w��d�;�N���d�;eh��`��w��N0�	�;�x'�d�ɀw2��x'�ɀw���	�;�x'��`��w��N0��.���d�;�N����w��N0�	�;�x'��`��]x'�ɀw2��x'�)C{'��`��w��N0�	�;�x'��N���d�;�N���|��w��N0�	�;�x'��`��]x'�ɀw2��x'�i~��	�;�x'��`��w��N0��.���d�;�N���4?_��`��w��N0�	�;�x'�d�ɀw2��x'�ɀw���w��N0�	�;�x'��`��w��d�;�N���d�;���;�x'��`��w��N0�	�;مw2��x'�ɀw2�����`��w��N0�	�;�x'����;�N���d�;�;�O{'(���	J��w��;A靠�NPy'���Nry'���\�I.�$�w��;����N`y'��X�	,��w�;YQ�I.�$�w��;����N��z'��X�	,��w�;����N`y'+�;����Nry'���\�i|^��w�;����N`y'��X�	,�dEy'���\�I.�$�w��;�����N`y'��X�	,��w�;�坬(�$�w��;����Nry��y�X�	,��w�;����N`y'������Nry'���\�I.�4>�w�;����N`y'��X�	,����;�N���d�;�N���N0�	�;�x'��`��w��Nv�x'�ɀw2��x���z'��`��w��N0�	�;�x'��N���d�;�N�S��N0�	�;�x'��`��w��Nv�x'�ɀw2��x��`��w��N0�	�;�x'����;�N���d�;�N�;�x'��`��w��N0�	�;مw2��x'�ɀw2��2�w��N0�	�;�x'��`��w��d�;�N���d�;eh��`��w��N0�	�;�x'�d�ɀw2��x'�ɀw��w��N0�	�;�x'��`��w��d�;�N���d�;eh��`��w��N0�	�;�x'�d�ɀw2��x'�ɀw���	�;�x'��`��w��N0��.���d�;�N����w��N0�	�;�x'��`��]x'�ɀw2��x'�)C{'��`��w��N0�	�;�x'��N���d�;�N�S��N0�	�;�x'��`��w��Nv�x'�ɀw2��x���z'��`��w��N0�	�;�x'��N���d�;�N���|��w��N0�	�;�x'��`��]x'�ɀw2��x'�i|��	�;�x'��`��w��N0��.���d�;�N���4>_��`��w��N0�	�;�x'�d�ɀw2��x'�ɀw��w��N0�	�;�x'��`��w��d�;�N���d�;���;�x'��`��w��N0�	�;مw2��x'�ɀw2����,�����6      1   e   x�3��,J�5�442639��/�45�0633252��;��&f��%��rAD� :<A:��:̌��L��,����b0XÑp�&&���f�y$1z\\\ ��-.      3   \   x�3�t�Wp��sr���4�4�t��vr�2��s���U�8����5D������s�|]��]��l@Vl�����������,���� �V�     