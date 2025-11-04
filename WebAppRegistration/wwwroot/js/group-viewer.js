const GroupViewer = () => {
    const [groupData, setGroupData] = React.useState(null);
    const [loading, setLoading] = React.useState(true);
    const [error, setError] = React.useState(null);

    React.useEffect(() => {
        const fetchGroupData = async () => {
            try {
                const pathParts = window.location.pathname.split('/');
                const slug = pathParts[pathParts.length - 1];

                const response = await fetch(`/api/group/${slug}`);
                if (!response.ok) {
                    throw new Error(`Групу не знайдено (статус: ${response.status})`);
                }
                const data = await response.json();
                setGroupData(data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchGroupData();
    }, []);

    if (loading) {
        return <h2>Завантаження...</h2>;
    }

    if (error) {
        return <div className="alert alert-danger">{error}</div>;
    }

    if (!groupData) {
        return null;
    }

    return (
        <div>
            <div className="text-center mb-4">
                {groupData.imageUrl && <img src={groupData.imageUrl} alt={groupData.name} className="img-fluid rounded mb-3" style={{ maxHeight: '300px' }} />}
                <h1>{groupData.name}</h1>
                <p className="lead">{groupData.description}</p>
            </div>

            <hr />

            <div className="row mt-4">
                {groupData.products.map(product => (
                    <div className="col-md-4 mb-4" key={product.id}>
                        <div className="card h-100 bg-dark text-white">
                            {product.imageUrl && <img src={product.imageUrl} className="card-img-top" alt={product.name} />}
                            <div className="card-body">
                                <h5 className="card-title">{product.name}</h5>
                                <h6 className="card-subtitle mb-2 text-muted">{new Intl.NumberFormat('uk-UA', { style: 'currency', currency: 'UAH' }).format(product.price)}</h6>
                            </div>
                            <div className="card-footer">
                                <a href={`/Product/Details/${product.id}`} className="btn btn-primary w-100">Детальніше</a>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

ReactDOM.render(<GroupViewer />, document.getElementById('react-app'));