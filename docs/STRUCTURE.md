Music-Playlist-Manager/
├── .github/                                # CI/CD
│   └── workflows/
│       └── dotnet.yml
├── data/
│   ├── raw/
│   │   └── songs_dataset.csv
│   └── user/
│       └── VARIES (type .csv)
├── docs/
│   └── STRUCTURE.md
├── src/
│   └── cli/
│       ├── DataStructures/
│       │   ├── LinkedList.cs
│       │   └── Node.cs
│       ├── Models/
│       │   └── Song.cs
│       ├── Utils/
│       │   ├── CSVLoader.cs
│       │   └── InputHandler.cs
│       ├── cli.csproj
│       ├── Constants.cs
│       ├── Program.cs                    
│       └── UsePlaylistManager.cs
├── tests/                                  # Integration tests
│   ├── tests.csproj
│   └── UnitTest1.cs
├── .gitignore                              # Files to exclude from version control
└── README.md                               # Project overview and setup guide