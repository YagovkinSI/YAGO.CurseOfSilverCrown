interface IRedirectProps {
    route : string
}

const Redirect: React.FC<IRedirectProps> = (props) => {
    window.location.replace(props.route);
    return null;
}

export default Redirect;