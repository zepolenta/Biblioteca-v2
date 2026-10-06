CREATE TABLE IF NOT EXISTS Livro (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Titulo VARCHAR(255),
    AutorID INT,
    AnoPublicacao INT,
    EditoraID INT,
    CategoriaID INT,
    ISBN VARCHAR(20),
    QuantidadeDisponivel INT,
    FOREIGN KEY (AutorID) REFERENCES Autor(ID),
    FOREIGN KEY (EditoraID) REFERENCES Editora(ID),
    FOREIGN KEY (CategoriaID) REFERENCES Categoria(ID)
);