import { Button } from "@mui/material";
import React from "react";
import { Link } from "react-router-dom";

interface ButtonWithLinkProps {
    to: string;
    text: string;
}

const ButtonWithLink: React.FC<ButtonWithLinkProps> = (prop) => {
    const isDesable = prop.to == "";

    return (
        <Link to={prop.to} style={{ textDecoration: 'none', color: 'inherit' }}>
            <Button variant="outlined" style={{ margin: '0.5rem' }} disabled={isDesable} >
                {prop.text}
            </ Button >
        </Link>
    )
}

export default ButtonWithLink
