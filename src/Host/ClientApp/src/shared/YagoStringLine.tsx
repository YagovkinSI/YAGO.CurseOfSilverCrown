import { Link, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { type JSX, type MouseEvent } from 'react';

export interface YagoStringLineProps {
    isTitleH1?: boolean,
    name: string,
    path?: string | undefined,
    isLinkToRazor?: boolean | undefined
}

const YagoStringLine: React.FC<YagoStringLineProps> = ({ name, path, isLinkToRazor, isTitleH1 }) => {
    const navigate = useNavigate();

    const handleClick = (e: MouseEvent<HTMLSpanElement>) => {
        if (!path) return;
        e.preventDefault();
        if (!isLinkToRazor) {
            navigate(path);
        } else {
            window.location.href = path;
        }
    };

    const renderStringLine = () : JSX.Element | string => {
        return (
            path
                ?
                <Link
                    component="span"
                    onClick={handleClick}
                    sx={{
                        color: 'inherit',
                        textDecoration: 'none',
                        '&:hover': {
                            textDecoration: 'underline'
                        }
                    }}
                >
                    {name}
                </Link>
                : name
        )
    }

    const fontWeight = isTitleH1 ? 'bold' : 'normal';

    return (
        <Typography
            component={isTitleH1 ? 'h1' : 'p'}
            variant={isTitleH1 ? "h6" : "inherit"}
            sx={{
                textDecoration: 'none',
                flexGrow: 1,
                textAlign: 'center',
                mx: 6,
                fontWeight: fontWeight,
                color: (theme) => path
                    ? theme.palette.primary.main
                    : theme.palette.text.primary,
                cursor: path ? 'pointer' : 'default',
                '&:hover': {
                    textDecoration: path ? 'underline' : 'none'
                }
            }}
        >
            {renderStringLine()}
        </Typography>
    )
}

export default YagoStringLine;