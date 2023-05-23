import { Link } from "react-router-dom";

export default function HomePage() {
    return (
        <div className="mx-auto max-w-7xl px-4 py-40 sm:px-6 lg:px-8">
            <div className="container flex flex-col gap-6">
                <h1 className="text-4xl md:text-5xl lg:text-6xl text-center font-bold tracking-tight text-gray-900">Easily determine <br /> text similarity.</h1>
                <p className="text-center font-medium tracking-tight text-gray-800">With the Text Similarity API, you can easily determine the similarity between two pieces of text with a free {' '}
                    <Link to="/sign-in" className=" underline underline-offset-2 font-semibold leading-6 text-indigo-600 hover:text-indigo-500">
                        API key.
                    </Link>
                </p>
            </div>
        </div>
    );
}