import { Box, Card, CardContent, CardMedia, IconButton } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import CloseIcon from '@mui/icons-material/Close';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import YagoStringLine from './YagoStringLine';

interface YagoCardProps {
    children?: React.ReactNode;
    title: string,
    path?: string | undefined,
    isLinkToRazor?: boolean | undefined
    image?: string | undefined;
    headerButtonsAccess?: boolean;
    handleBack?: (() => void) | undefined
}

const YagoCard: React.FC<YagoCardProps> = ({ children, title, path, isLinkToRazor, image = undefined, headerButtonsAccess: mainButtonsAvalilable = true, handleBack = undefined }) => {
    const navigate = useNavigate();

    const renderBackButton = () => {
        return (
            <IconButton
                aria-label="Назад"
                onClick={() => handleBack ? handleBack() : navigate(-1)}
                sx={{ position: 'absolute', left: 8 }}
            >
                <ArrowBackIcon />
            </IconButton>
        )
    }

    const renderTitle = () => {
        return (
            <YagoStringLine
                name={title}
                path={path}
                isLinkToRazor={isLinkToRazor}
                isTitleH1={true}
            />
        )
    }

    const renderCloseButton = () => {
        return (
            <IconButton
                aria-label="Закрыть"
                onClick={() => navigate('/')}
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
                    alignItems: 'center',
                    padding: { sx: 0, sm: 2 },
                    minHeight: '32px',
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
                boxShadow: '5px 5px 5px rgba(0, 0, 0, .5)'
            }}>
            {cardHeader()}
            {image != undefined
                ? cardImage
                : <></>
            }
            <CardContent 
                sx={{ 
                    padding: { xs: '8px', sm: '16px' }
                }}>
                {children}
            </CardContent>
        </Card>
    )
}

export default YagoCard