import { Card, CardContent, CardMedia } from '@mui/material';

interface YagoCardProps {
    children: React.ReactNode;
    image: string | undefined;
}

const YagoCard: React.FC<YagoCardProps> = ({ children, image}) => {

    const cardImage = <CardMedia
        component="img"
        image={image}
        alt="Yago picture" />;

    return (
        <Card
            style={{
                backgroundColor: 'rgba(255, 255, 255, 0.85)',
                borderRadius: 2,
                maxWidth: '80vh',
                margin: 'auto',
                marginBottom: '1rem',
                padding: '1rem',
            }}>
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