import { Box, Card, CardContent, CardMedia, IconButton, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import CloseIcon from '@mui/icons-material/Close';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';

interface YagoCardProps {
    children?: React.ReactNode;
    title: string;
    titleLink?: string | undefined;
    image?: string | undefined;
    headerButtonsAccess?: boolean
}

const YagoCard: React.FC<YagoCardProps> = ({ title, titleLink = null, children, image = undefined, headerButtonsAccess: mainButtonsAvalilable = true }) => {
    const navigate = useNavigate();

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

    const renderTitle = () => {
        return (
            <Typography
                component="h1"
                variant="h6"
                sx={{
                    flexGrow: 1,
                    textAlign: 'center',
                    mx: 6
                }}
            >
                {titleLink == undefined
                    ? <>{title}</>
                    : <a href={titleLink}>{title}</a>
                }
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
                backgroundColor: 'rgba(255, 255, 255, 0.85)',
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