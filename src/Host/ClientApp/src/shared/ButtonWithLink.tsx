import React from "react";
import { Link } from "react-router-dom";
import YagoButton from "./YagoButton";

interface ButtonWithLinkProps {
    to: string;
    text: string;
}

const ButtonWithLink: React.FC<ButtonWithLinkProps> = (prop) => {
    const isDisabled = prop.to == "";

    return (
        <Link to={prop.to} style={{ textDecoration: 'none', color: 'inherit' }}>
            <YagoButton onClick={undefined} text={prop.text} isDisabled={isDisabled}  />
        </Link>
    )
}

export default ButtonWithLink
