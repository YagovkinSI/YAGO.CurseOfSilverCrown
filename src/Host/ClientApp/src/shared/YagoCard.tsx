import { Box, Card, CardContent, CardMedia, IconButton, Link, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import CloseIcon from '@mui/icons-material/Close';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import type YagoLink from '../entities/YagoLink';
import { type MouseEvent } from 'react';

interface YagoCardProps {
    children?: React.ReactNode;
    title: YagoLink;
    image?: string | undefined;
    headerButtonsAccess?: boolean
}

const YagoCard: React.FC<YagoCardProps> = ({ title, children, image = undefined, headerButtonsAccess: mainButtonsAvalilable = true }) => {
    const navigate = useNavigate();

    const handleClick = (e: MouseEvent<HTMLSpanElement>) => {
        if (!title.path) return;
        e.preventDefault();
        if (!title.isLinkToRazor) {
            navigate(title.path);
        } else {
            window.location.href = title.path;
        }
    };

    const renderBackButton = () => {
        return (
            <IconButton
                aria-label="Назад"
                onClick={() => navigate(-1)}
                sx={{ position: 'absolute', left: 8 }}
            >
                <ArrowBackIcon />
            </IconButton>
        )
    }

    const renderTittleLink = () => {
        return title.path
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
                {title.name}
            </Link>
            : title.name

    }

    const renderTitle = () => {
        return (
            <Typography
                component="h1"
                variant="h6"
                sx={{
                    flexGrow: 1,
                    textAlign: 'center',
                    mx: 6,
                    fontWeight: 'bold',
                    color: (theme) => title.path
                        ? theme.palette.primary.main
                        : theme.palette.text.primary,
                    cursor: title.path ? 'pointer' : 'default',
                    '&:hover': {
                        textDecoration: title.path ? 'underline' : 'none'
                    }
                }}
            >
                {renderTittleLink()}
            </Typography>
        )
    }

    const renderCloseButton = () => {
        return (
            <IconButton
                aria-label="Закрыть"
                onClick={() => navigate('/app/map')}
                sx={{ position: 'absolute', right: 8 }}
            >
                <CloseIcon />
            </IconButton>
        )
    }

    const cardHeader = () => {
        return (
            <Box
                component="header"
                sx={{
                    display: 'flex',
                    alignItems: '',
                    p: 2,
                    position: 'relative'
                }}
            >
                {mainButtonsAvalilable && renderBackButton()}
                {renderTitle()}
                {mainButtonsAvalilable && renderCloseButton()}
            </Box>
        );
    }

    const cardImage = <CardMedia
        component="img"
        image={image}
        alt="Yago picture" />;

    return (
        <Card
            style={{
                backgroundColor: 'var(--color-light-a09)',
                maxWidth: '80vh',
                margin: 'auto',
            }}>
            {cardHeader()}
            {image != undefined
                ? cardImage
                : <></>
            }
            <CardContent>
                {children}
            </CardContent>
        </Card>
    )
}

export default YagoCard