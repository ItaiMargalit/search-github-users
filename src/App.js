import React, {useState, useEffect} from 'react';
import './App.css';

function App() {
    const [inputValue, setInputValue] = useState('');
    const [query, setQuery] = useState('');
    const [users, setUsers] = useState([]);
    const [error, setError] = useState(null);
    const [totalUsers, setTotalUsers] = useState(0);
    const [page, setPage] = useState(1);
    const [loading, setLoading] = useState(false);
    const [nextPageLoading, setNextPageLoading] = useState(false);

    const setStates = ({loading = false, users = [], page = 1, error = null, totalUsers = 0}) => {
        setLoading(loading);
        setUsers(users);
        setPage(page);
        setError(error);
        setTotalUsers(totalUsers);
    };

    const handleSearch = async (reset = true) => {
        try {
            setStates({loading: true});
            const response = await fetch(`https://localhost:7175/api/githubsearch/search_github_users?q=${encodeURIComponent(query)}&page=1&per_page=50`,);
            if (!response.ok) {
                throw new Error(`Error: ${response.status} ${response.statusText}`);
            }

            const data = await response.json();
            setStates({users: data.users || [], totalUsers: data.total_count});

        } catch (err) {
            console.error("Error fetching data:", err);
            setStates({error: "Error occured while fetching data"});
        }
    };

    const loadNextPage = async () => {
        try {
            setNextPageLoading(true);
            const nextPage = page + 1;
            const response = await fetch(`https://localhost:7175/api/githubsearch/search_github_users?q=${encodeURIComponent(query)}&page=${nextPage}&per_page=50`,);
            if (response.ok) {
                const data = await response.json();
                setStates({
                    loading: true,
                    users: prevUsers => [...prevUsers, ...data.users],
                    page: nextPage,
                    totalUsers: data.total_count
                })
            }
        } catch (err) {
            console.error("Error loading next page:", err);
        } finally {
            setNextPageLoading(false);
        }
    };

    useEffect(() => {
        if (query) {
            handleSearch();
        }
    }, [query]);

    useEffect(() => {
        const handleScroll = () => {
            if (window.innerHeight + document.documentElement.scrollTop >= document.documentElement.offsetHeight - 100 && !nextPageLoading) {
                loadNextPage();
            }
        };

        window.addEventListener("scroll", handleScroll);
        return () => window.removeEventListener("scroll", handleScroll);
    }, [nextPageLoading, page]);

    useEffect(() => {
        const delayDebounce = setTimeout(() => {
            setQuery(inputValue);
        }, 500);

        return () => clearTimeout(delayDebounce);
    }, [inputValue]);

    const handleInputChange = (e) => {
        setInputValue(e.target.value);
    };

    return (
        <div className="App">
            <h1>GitHub Users Search</h1>
            <h2>By Itai Margalit</h2>

            <p className="total-users">Total Users: {totalUsers}</p>

            <div className="search-input-container">
                <input
                    type="text"
                    placeholder="Search GitHub users..."
                    value={inputValue}
                    onChange={handleInputChange}
                    className="search-input"
                />
            </div>

            {error && <p style={{color: 'red'}}>{error}</p>}

            <div className="grid-container">
                {users.map((user) => (
                    <div
                        key={user.username + user.id}
                        onClick={() => window.open(`https://github.com/${user.username}`, '_blank')}
                        className="user-card"
                    >
                        <div className="badge">{user.publicRepos}</div>
                        {user.image ? (
                            <img src={user.image} alt={user.username}/>
                        ) : (
                            <div className="no-picture">No Picture</div>
                        )}
                        <p className="user-name">{user.username}</p>
                    </div>
                ))}
            </div>

            {loading && <p>Loading users...</p>}
        </div>
    );
}

export default App;
