CREATE TABLE partita (
	codice_unet TEXT PRIMARY KEY,
	inizio TEXT, -- YYYY-MM-DD HH:MM:SS.SSS
	fine TEXT, -- YYYY-MM-DD HH:MM:SS.SSS
	abortita BOOLEAN
);

CREATE TABLE utente (
	id_utente INTEGER PRIMARY KEY AUTOINCREMENT,
	facebook_key TEXT,
	descrizione TEXT,
	unique (facebook_key)
);

CREATE TABLE partecipanti (
	codice_unet TEXT,
	id_utente INTEGER,
	punti INTEGER,
	posizione INTEGER,
	
	UNIQUE(codice_unet, id_utente),
	FOREIGN KEY (id_utente) REFERENCES utente(id_utente)
);
