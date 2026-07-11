import { ArrowLeft } from "lucide-react";
import { useNavigate } from "react-router-dom";

const ButtonBack: React.FC = () => {
    const navigate = useNavigate();

    return <button
        onClick={() => navigate(-1)}
        className="p-2 text-muted hover:text-light transition-colors"
    >
        <ArrowLeft className="w-5 h-5" />
    </button>
};

export default ButtonBack;